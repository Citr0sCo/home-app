import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { first, Subject, Subscription, takeUntil } from 'rxjs';
import { environment } from '../../environments/environment';

@Component({
    selector: 'url-health-checker',
    templateUrl: './url-health-checker.component.html',
    styleUrls: ['./url-health-checker.component.scss']
})
export class UrlHealthCheckerComponent implements OnInit, OnDestroy {

    @Input()
    public url: string = '';

    @Input()
    public host: string = '';

    @Input()
    public port: number = 0;

    @Input()
    public isSecure: boolean = false;

    public status: string = 'unknown';

    public statusDescription: string = 'Unknown state';

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public ngOnInit(): void {
        this._httpClient.get(`${environment.apiBaseUrl}/api/healthcheck?url=${this.host}:${this.port}&isSecure=${this.isSecure}`, {})
            .pipe(
                first(),
                takeUntil(this._destroy)
            )
            .subscribe((response: any) => {
                if (response.StatusCode === 200) {
                    this.status = 'up';
                    this.statusDescription = 'Service is reachable.';
                } else if (response.StatusCode.toString()[0] === '4') {
                    this.status = 'warning';
                    this.statusDescription = `Service has returned an '${response.StatusDescription}' response.`;
                } else {
                    this.status = 'down';
                    this.statusDescription = response.StatusText;
                }
            }, (error) => {
                this.status = 'down';
                this.statusDescription = 'Service is down.';
                console.error(error);
            });
    }

    public ngOnDestroy(): void {
        this._destroy.next();
    }
}
