import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subject, takeUntil } from 'rxjs';
import { LinkService } from '../../services/link-service/link.service';
import { IStatResponse } from '../../services/stats-service/types/stat.response';
import { StatService } from '../../services/stats-service/stat.service';
import { IBuild } from '../../services/build-service/types/build.type';
import { WebSocketService } from '../../services/websocket-service/web-socket.service';
import { WebSocketKey } from '../../services/websocket-service/types/web-socket.key';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';
import { IColumn } from '../../services/link-service/types/column.type';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ILink } from '../../services/link-service/types/link.type';

@Component({
    selector: 'links',
    templateUrl: './links.component.html',
    styleUrls: ['./links.component.scss'],
    standalone: false
})
export class LinksComponent implements OnInit, OnDestroy {

    public columns: Array<IColumn> = [];

    public currentTime: Date = new Date();
    public builds: Array<IBuild> = [];
    public isEditModeEnabled: boolean = false;
    public allStats: Array<IStatModel> = new Array<IStatModel>();
    public refreshCache: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public showWidgets: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    private readonly _linkService: LinkService;
    private readonly _statService: StatService;
    private readonly _webSocketService: WebSocketService;
    private readonly _destroy: Subject<void> = new Subject();

    constructor(linkService: LinkService, statService: StatService) {
        this._linkService = linkService;
        this._statService = statService;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._statService.getAll()
            .pipe(takeUntil(this._destroy))
            .subscribe((response: IStatResponse | null) => {
                this.allStats = response?.stats ?? new Array<IStatModel>();
            });

        this._statService.stats
            .asObservable()
            .subscribe((response: IStatResponse | null) => {
                this.allStats = response?.stats ?? new Array<IStatModel>();
            });

        this._linkService.getAllColumns()
            .pipe(takeUntil(this._destroy))
            .subscribe((columns) => {
                this.columns = columns;
            });

        const showWidgets = localStorage.getItem('showWidgets');
        if (showWidgets !== null) {
            this.showWidgets.next(showWidgets === 'true');
        }

        this.showWidgets
            .pipe(takeUntil(this._destroy))
            .subscribe((showWidgets) => {
                localStorage.setItem('showWidgets', showWidgets.toString());
            });

        setInterval(() => {
            this.currentTime = new Date();
        }, 1000);

        this._webSocketService.send(WebSocketKey.Handshake, { Test: 'Hello World!' });

        this._statService.ngOnInit();
    }

    public refreshLinkCache(): void {

        this.refreshCache.next(true);

        this._linkService.getUpdatedColumns()
            .pipe(takeUntil(this._destroy))
            .subscribe((columns) => {
                this.columns = columns;
            });

        this._linkService.refreshCache()
            .pipe(takeUntil(this._destroy))
            .subscribe();
    }

    public createColumn(): void {

        const request = {
            identifier: null,
            name: `Column ${this.columns.length + 1}`,
            icon: 'fas fa-file-alt',
            sortOrder: this.columns.length,
            links: []
        } as IColumn;

        this._linkService.createColumn(request)
            .pipe(takeUntil(this._destroy))
            .subscribe(() => {
                this.refreshLinkCache();
            });
    }

    public toggleWidgets(): void {
        this.showWidgets.next(!this.showWidgets.value);
    }

    public persistChanges(): void {

        this.isEditModeEnabled = !this.isEditModeEnabled;

        if (this.isEditModeEnabled) {
            return;
        }

        this.columns = this.columns.map((column) => {

            column.links = column.links.map((link, index) => {
                link.sortOrder = index;
                return link;
            });

            return column;
        });

        this._linkService.importColumns(this.columns).subscribe(() => {
            this.refreshLinkCache();
        });
    }

    public drop($event: CdkDragDrop<Array<string>>): void {
        moveItemInArray(this.columns, $event.previousIndex, $event.currentIndex);

        this.columns = this.columns.map((column, index) => {
            column.sortOrder = index;
            return column;
        });

    }

    public ngOnDestroy(): void {
        this._statService.ngOnDestroy();
        this._destroy.next();
    }
}
