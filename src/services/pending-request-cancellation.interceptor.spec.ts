import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { PendingRequestCancellationService } from './pending-request-cancellation.service';
import { pendingRequestCancellationInterceptor } from './pending-request-cancellation.interceptor';

describe('pendingRequestCancellationInterceptor', () => {
    let http: HttpTestingController;
    let cancellationService: PendingRequestCancellationService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                PendingRequestCancellationService,
                provideHttpClient(withInterceptors([pendingRequestCancellationInterceptor])),
                provideHttpClientTesting()
            ]
        });

        http = TestBed.inject(HttpTestingController);
        cancellationService = TestBed.inject(PendingRequestCancellationService);
    });

    afterEach(() => {
        http.verify();
        TestBed.resetTestingModule();
    });

    it('cancels an in-flight HTTP request when navigation starts', () => {
        const subscription = TestBed.inject(HttpClient).get('/api/pending').subscribe();
        const request = http.expectOne('/api/pending');

        cancellationService.cancelAll();

        expect(request.cancelled).toBe(true);
        subscription.unsubscribe();
    });
});
