import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, finalize, Subscriber, Subscription } from 'rxjs';
import { environment } from '../../environments/environment';

interface QueuedHealthCheck {
    url: string;
    isSecure: boolean;
    linkReference: string | null;
    subscriber: Subscriber<any>;
    subscription: Subscription | null;
    cancelled: boolean;
}

@Injectable({ providedIn: 'root' })
export class HealthCheckService {

    private static readonly _MAX_CONCURRENT_CHECKS = 4;

    private readonly _httpClient: HttpClient;
    private readonly _queue: Array<QueuedHealthCheck> = [];
    private _activeChecks = 0;
    private _queueProcessingScheduled = false;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public check(url: string, isSecure: boolean, linkReference: string | null = null): Observable<any> {
        return new Observable<any>((subscriber) => {
            const request: QueuedHealthCheck = {
                url,
                isSecure,
                linkReference,
                subscriber,
                subscription: null,
                cancelled: false
            };

            this._queue.push(request);
            this.scheduleQueueProcessing();

            return () => {
                request.cancelled = true;

                const queuedIndex = this._queue.indexOf(request);
                if (queuedIndex !== -1) {
                    this._queue.splice(queuedIndex, 1);
                }

                request.subscription?.unsubscribe();
            };
        });
    }

    private scheduleQueueProcessing(): void {
        if (this._queueProcessingScheduled) {
            return;
        }

        this._queueProcessingScheduled = true;
        setTimeout(() => {
            this._queueProcessingScheduled = false;
            this.processQueue();
        });
    }

    private processQueue(): void {
        while (this._activeChecks < HealthCheckService._MAX_CONCURRENT_CHECKS && this._queue.length > 0) {
            const request = this._queue.shift()!;

            if (request.cancelled) {
                continue;
            }

            this._activeChecks++;
            let params = new HttpParams()
                .set('url', request.url)
                .set('isSecure', request.isSecure);

            if (request.linkReference) {
                params = params.set('linkReference', request.linkReference);
            }

            request.subscription = this._httpClient.get(`${environment.apiBaseUrl}/api/healthcheck`, { params })
                .pipe(
                    finalize(() => {
                        this._activeChecks--;
                        this.processQueue();
                    })
                )
                .subscribe({
                    next: (response) => request.subscriber.next(response),
                    error: (error) => request.subscriber.error(error),
                    complete: () => request.subscriber.complete()
                });
        }
    }
}
