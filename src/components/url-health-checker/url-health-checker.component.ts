import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { first, Subscription } from "rxjs";

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

    private _subscriptions: Subscription = new Subscription();

    private readonly _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public ngOnInit(): void {
        this._subscriptions.add(
            this._httpClient.get(`/api/healthcheck?url=${this.host}:${this.port}&isSecure=${this.isSecure}`, {})
                .pipe(first())
                .subscribe((response: any) => {
                    if (response.statusCode === 200) {
                        this.status = 'up';
                        this.statusDescription = 'Service is reachable.';
                    } else if (response.statusCode.toString()[0] === '4') {
                        this.status = 'warning';
                        this.statusDescription = `Service has returned an '${response.statusDescription}' response.`;
                    } else {
                        this.status = 'down';
                        this.statusDescription = response.statusText;
                    }
                }, (error) => {
                    this.status = 'down';
                    this.statusDescription = error.statusText;
                    console.error(error);
                })
        );
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }
}
