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

    public status: string = 'unknown';

    public statusDescription: string = 'Unknown state';

    private _subscriptions: Subscription = new Subscription();

    private readonly _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public handleProgress(progress: number, total: number): void {
        console.log(progress, total);
    }

    public ngOnInit(): void {
        this._subscriptions.add(
            this._httpClient.get(`${this.host}:${this.port}`, { observe: 'response' })
                .pipe(first())
                .subscribe((response) => {
                    if (response.status === 200) {
                        this.status = 'up';
                        this.statusDescription = 'Service is reachable';
                    } else {
                        this.status = 'down';
                        this.statusDescription = response.statusText;
                    }
                }, (error) => {
                    this.status = 'down';
                    this.statusDescription = error.statusText;
                    console.log(error);
                })
        );
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }
}
