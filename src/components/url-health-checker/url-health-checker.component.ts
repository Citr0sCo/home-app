import { Component, Input, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { first, Subject, takeUntil } from 'rxjs';
import { environment } from '../../environments/environment';

@Component({
    selector: 'url-health-checker',
    templateUrl: './url-health-checker.component.html',
    styleUrls: ['./url-health-checker.component.scss'],
    standalone: false
})
export class UrlHealthCheckerComponent implements OnInit, OnDestroy {

    @Input()
    public url: string = '';

    @Input()
    public host: string = '';

    @Input()
    public port: number = 0;

    public isSecure: WritableSignal<boolean> = signal<boolean>(false);

    public status: WritableSignal<string> = signal<string>('');

    public statusDescription: WritableSignal<string> = signal<string>('Unknown state');

    public responseTime: WritableSignal<number> = signal<number>(0);

    public isLoading: WritableSignal<boolean> = signal<boolean>(true);

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public ngOnInit(): void {

        this.isSecure.set(this.url.startsWith('https://'));

            this._httpClient.get(`${environment.apiBaseUrl}/api/healthcheck?url=${this.host}:${this.port}&isSecure=${this.isSecure()}`, {})
            .pipe(
                first(),
                takeUntil(this._destroy)
            )
            .subscribe({
                next: (response: any) => {
                    if (response.StatusCode.toString()[0] === '2' || response.StatusCode.toString()[0] === '3') {
                        this.status.set('up');
                        this.statusDescription.set('Service is reachable.');
                    } else if (response.StatusCode.toString()[0] === '4') {
                        this.status.set('warning');
                        this.statusDescription.set(`Service has returned an '${response.StatusDescription}' response.`);
                    } else {
                        this.status.set('down');
                        this.statusDescription.set(response.StatusDescription);
                    }
                    this.responseTime.set(response.DurationInMilliseconds);
                    this.isLoading.set(false);
                },
                error: (error) => {
                    this.status.set('down');
                    this.statusDescription.set('Service is down.');
                    this.responseTime.set(0);
                    this.isLoading.set(false);
                    console.error(error);
                }
            });
    }

    public determineResponseTime(responseTime: number): string {

        if (responseTime >= 1000) {
            return `${Math.round((responseTime / 1000) * 100) / 100} s`;
        }

        return `${responseTime} ms`;
    }

    public ngOnDestroy(): void {
        this._destroy.next();
    }
}
