import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Subscription } from 'rxjs';
import { HealthCheckService } from './healthcheck.service';

describe('HealthCheckService', () => {
    let service: HealthCheckService;
    let http: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                HealthCheckService,
                provideHttpClient(),
                provideHttpClientTesting()
            ]
        });

        service = TestBed.inject(HealthCheckService);
        http = TestBed.inject(HttpTestingController);
        vi.useFakeTimers();
    });

    afterEach(() => {
        http.verify();
        TestBed.resetTestingModule();
        vi.useRealTimers();
    });

    it('defers the first requests until the current render completes', () => {
        const subscription = service.check('example.com', false).subscribe();

        expect(http.match((testRequest) => testRequest.url.endsWith('/api/healthcheck'))).toHaveLength(0);

        vi.runOnlyPendingTimers();

        const request = http.expectOne((testRequest) =>
            testRequest.url.endsWith('/api/healthcheck') &&
            testRequest.params.get('url') === 'example.com' &&
            testRequest.params.get('isSecure') === 'false'
        );
        request.flush({});
        subscription.unsubscribe();
    });

    it('limits concurrent checks and starts queued checks as requests finish', () => {
        const subscriptions = createChecks(5);

        vi.runOnlyPendingTimers();

        const requests = http.match((testRequest) => testRequest.url.endsWith('/api/healthcheck'));
        expect(requests).toHaveLength(4);

        requests[0].flush({});

        const queuedRequest = http.expectOne((testRequest) =>
            testRequest.url.endsWith('/api/healthcheck') &&
            testRequest.params.get('url') === 'example-4.com'
        );
        queuedRequest.flush({});

        subscriptions.forEach((subscription) => subscription.unsubscribe());
    });

    it('cancels checks that are still waiting in the queue', () => {
        const subscriptions = createChecks(5);
        subscriptions[4].unsubscribe();

        vi.runOnlyPendingTimers();

        expect(http.match((testRequest) => testRequest.url.endsWith('/api/healthcheck'))).toHaveLength(4);

        subscriptions.forEach((subscription) => subscription.unsubscribe());
    });

    function createChecks(count: number): Array<Subscription> {
        return Array.from({ length: count }, (value, index) =>
            service.check(`example-${index}.com`, false).subscribe()
        );
    }
});
