import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { LinkRepository } from '../../services/link-service/link.repository';
import { LinkService } from '../../services/link-service/link.service';
import { PendingRequestCancellationService } from '../../services/pending-request-cancellation.service';
import { ILink } from '../../services/link-service/types/link.type';
import { CustomLinkComponent } from './custom-link.component';

describe('CustomLinkComponent', () => {
    let linkService: LinkService;
    let http: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                LinkService,
                LinkRepository,
                PendingRequestCancellationService,
                provideHttpClient(),
                provideHttpClientTesting()
            ]
        });

        linkService = TestBed.inject(LinkService);
        http = TestBed.inject(HttpTestingController);
    });

    afterEach(() => {
        http.verify();
        TestBed.resetTestingModule();
        vi.unstubAllGlobals();
    });

    it('waits for the backend to record a click before navigating', () => {
        const identifier = 'a3e832d1-1507-4abc-aebc-9d9bf640f001';
        const url = 'https://example.com';
        const assign = vi.fn();
        vi.stubGlobal('window', { location: { assign } });

        const component = new CustomLinkComponent(
            linkService,
            TestBed.inject(PendingRequestCancellationService)
        );
        component.item = { identifier, url, lastClickedAt: null } as ILink;
        const event = { preventDefault: vi.fn(), stopPropagation: vi.fn() } as unknown as MouseEvent;

        component.recordClick(event);

        const request = http.expectOne((testRequest) =>
            testRequest.url.endsWith(`/api/links/${identifier}/click`)
        );
        expect(request.request.method).toBe('POST');
        expect(event.preventDefault).toHaveBeenCalledOnce();
        expect(event.stopPropagation).toHaveBeenCalledOnce();
        expect(component.item.lastClickedAt).toEqual(expect.any(String));
        expect(assign).not.toHaveBeenCalled();

        request.flush({ HasError: false });

        expect(assign).toHaveBeenCalledWith(url);
    });
});
