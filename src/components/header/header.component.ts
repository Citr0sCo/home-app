import { Component, ElementRef, OnDestroy, OnInit, QueryList, signal, ViewChild, ViewChildren, WritableSignal } from '@angular/core';
import { Subscription } from 'rxjs';
import { LinkService } from '../../services/link-service/link.service';
import { IColumn } from '../../services/link-service/types/column.type';
import { WebSocketService } from '../../services/websocket-service/web-socket.service';
import { WebSocketKey } from '../../services/websocket-service/types/web-socket.key';
import { findLinkSearchResults, ILinkSearchResult } from './link-search';

@Component({
    selector: 'header-component',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss'],
    standalone: false
})
export class HeaderComponent implements OnInit, OnDestroy {

    @ViewChild('dialogSearchInput')
    public dialogSearchInput!: ElementRef<HTMLInputElement>;

    @ViewChildren('searchResult')
    public searchResultElements!: QueryList<ElementRef<HTMLButtonElement>>;

    public currentTime: Date = new Date();
    public webQuery: string = '';
    public isConnected: WritableSignal<boolean> = signal<boolean>(false);
    public isSearchOpen: WritableSignal<boolean> = signal<boolean>(false);
    public searchResults: WritableSignal<Array<ILinkSearchResult>> = signal<Array<ILinkSearchResult>>([]);
    public selectedResultIndex: WritableSignal<number> = signal<number>(-1);

    private readonly _subscriptions: Subscription = new Subscription();
    private readonly _webSocketService: WebSocketService;
    private readonly _linkService: LinkService;
    private _columns: Array<IColumn> = [];

    constructor(linkService: LinkService) {
        this._linkService = linkService;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._subscriptions.add(
            this._linkService.getAllColumns().subscribe((columns) => {
                this._columns = columns;
                this.updateSearchResults();
            })
        );

        this._subscriptions.add(
            this._webSocketService.isConnected
                .asObservable()
                .subscribe((isConnected: boolean) => {
                    this.isConnected.set(isConnected);

                    if (!this.isConnected()) {
                        console.log('Attempting to reconnect to websocket in 5 seconds...');
                        setTimeout(() => {
                            if (location.href.indexOf('https') > -1 || location.href.indexOf('localhost') > -1) {
                                this._webSocketService.connect(true);
                            } else {
                                this._webSocketService.connect();
                            }
                        }, 5000);
                    }
                })
        );

        setInterval(() => {
            this.currentTime = new Date();
        }, 1000);

        this._webSocketService.send(WebSocketKey.Handshake, { Test: 'Hello World!' });
    }

    public getGreeting(): string {

        let greeting = 'Welcome';

        if (this.currentTime.getHours() < 12) {
            greeting = 'Good Morning';
        }
        if (this.currentTime.getHours() > 12) {
            greeting = 'Good Afternoon';
        }
        if (this.currentTime.getHours() > 18) {
            greeting = 'Good Evening';
        }

        return greeting;
    }

    public openSearch(): void {
        if (this.isSearchOpen()) {
            return;
        }

        this.isSearchOpen.set(true);
        this.updateSearchResults();
        setTimeout(() => this.dialogSearchInput?.nativeElement.focus());
    }

    public closeSearch(): void {
        this.isSearchOpen.set(false);
        this.selectedResultIndex.set(-1);
    }

    public updateSearchResults(): void {
        this.searchResults.set(findLinkSearchResults(this._columns, this.webQuery));
        this.selectedResultIndex.set(-1);
    }

    public handleSearchKeydown(event: KeyboardEvent): void {
        if (event.key === 'Escape') {
            event.preventDefault();
            this.closeSearch();
            return;
        }

        if (event.key === 'ArrowDown') {
            event.preventDefault();
            this.selectResult(0);
            this.focusSelectedResult();
            return;
        }

        if (event.key === 'Enter' && this.selectedResultIndex() >= 0) {
            event.preventDefault();
            this.openSelectedResult();
        }
    }

    public handleResultKeydown(event: KeyboardEvent, index: number): void {
        if (event.key === 'ArrowDown') {
            event.preventDefault();
            this.selectResult(Math.min(index + 1, this.searchResults().length));
            this.focusSelectedResult();
            return;
        }

        if (event.key === 'ArrowUp') {
            event.preventDefault();
            if (index === 0) {
                this.selectedResultIndex.set(-1);
                this.dialogSearchInput?.nativeElement.focus();
                return;
            }
            this.selectResult(index - 1);
            this.focusSelectedResult();
            return;
        }

        if (event.key === 'Enter') {
            event.preventDefault();
            this.openSelectedResult();
            return;
        }

        if (event.key === 'Escape') {
            event.preventDefault();
            this.closeSearch();
        }
    }

    public selectResult(index: number): void {
        this.selectedResultIndex.set(index);
    }

    public openResult(result: ILinkSearchResult): void {
        if (result.isGoogleSearch) {
            this.searchWeb();
            return;
        }

        if (result.link) {
            window.location.href = result.link.url;
        }
    }

    public searchWeb(): void {
        window.location.href = `https://www.google.com/search?q=${encodeURIComponent(this.webQuery)}`;
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }

    private openSelectedResult(): void {
        const index = this.selectedResultIndex();
        const result = this.searchResults()[index] ?? {
            link: null,
            columnName: null,
            folderName: null,
            isGoogleSearch: true
        };

        this.openResult(result);
    }

    private focusSelectedResult(): void {
        setTimeout(() => this.searchResultElements.get(this.selectedResultIndex())?.nativeElement.focus());
    }
}
