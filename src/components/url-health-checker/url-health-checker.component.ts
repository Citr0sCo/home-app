import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { catchError, map, of, Subscription } from "rxjs";

@Component({
    selector: 'url-health-checker',
    templateUrl: './url-health-checker.component.html',
    styleUrls: ['./url-health-checker.component.scss']
})
export class UrlHealthCheckerComponent implements OnInit, OnDestroy {

    @Input()
    public url: string = '';

    public status: string = 'unknown';

    public statusDescription: string = 'Unknown state';

    private _subscriptions: Subscription = new Subscription();

    private readonly _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public ngOnInit(): void {
        this._subscriptions.add(
            this._httpClient.get(this.url)
                .pipe(
                    map((payload: any) => {
                        console.log(payload);
                    }),
                    catchError((error: any) => {
                        console.log(error);
                        return of(error);
                    })
                )
                .subscribe((response) => {
                    this.status = 'up';
                    this.statusDescription = 'Service is reachable';
                }, (error) => {
                    this.status = 'down';
                    this.statusDescription = error.statusText;
                })
        );
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }
}
