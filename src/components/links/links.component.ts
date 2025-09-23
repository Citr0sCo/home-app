import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { LinkService } from '../../services/link-service/link.service';
import { ILink } from '../../services/link-service/types/link.type';
import { IStatResponse } from '../../services/stats-service/types/stat.response';
import { StatService } from '../../services/stats-service/stat.service';
import { IBuild } from '../../services/build-service/types/build.type';
import { WebSocketService } from '../../services/websocket-service/web-socket.service';
import { WebSocketKey } from '../../services/websocket-service/types/web-socket.key';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';
import { CdkDragDrop } from '@angular/cdk/drag-drop';

@Component({
    selector: 'links',
    templateUrl: './links.component.html',
    styleUrls: ['./links.component.scss'],
    standalone: false
})
export class LinksComponent implements OnInit, OnDestroy {

    public mediaLinks: Array<ILink> = [];
    public systemLinks: Array<ILink> = [];
    public productivityLinks: Array<ILink> = [];
    public toolsLinks: Array<ILink> = [];
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

        this._linkService.getAllLinks()
            .pipe(
                takeUntil(this._destroy),
                switchMap(() => {
                    return this._linkService.getMediaLinks()
                        .pipe(
                            takeUntil(this._destroy),
                            tap((links) => {
                                this.mediaLinks = links;
                            })
                        );
                }),
                switchMap(() => {
                    return this._linkService.getSystemLinks()
                        .pipe(
                            takeUntil(this._destroy),
                            tap((links) => {
                                this.systemLinks = links;
                            })
                        );
                }),
                switchMap(() => {
                    return this._linkService.getProductivityLinks()
                        .pipe(
                            takeUntil(this._destroy),
                            tap((links) => {
                                this.productivityLinks = links;
                            })
                        );
                }),
                switchMap(() => {
                    return this._linkService.getToolsLinks()
                        .pipe(
                            takeUntil(this._destroy),
                            tap((links) => {
                                this.toolsLinks = links;
                            })
                        );
                })
            )
            .subscribe();

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

    public getLastSortOrder(links: Array<ILink>): number {
        return links.length;
    }

    public refreshLinkCache(): void {

        this.refreshCache.next(true);

        this._linkService.getUpdatedLinks()
            .pipe(
                takeUntil(this._destroy),
                switchMap(() => {
                    return this._linkService.getMediaLinks()
                        .pipe(
                            takeUntil(this._destroy),
                            tap((links) => {
                                this.mediaLinks = links;
                            })
                        );
                }),
                switchMap(() => {
                    return this._linkService.getSystemLinks()
                        .pipe(
                            takeUntil(this._destroy),
                            tap((links) => {
                                this.systemLinks = links;
                            })
                        );
                }),
                switchMap(() => {
                    return this._linkService.getProductivityLinks()
                        .pipe(
                            takeUntil(this._destroy),
                            tap((links) => {
                                this.productivityLinks = links;
                            })
                        );
                }),
                switchMap(() => {
                    return this._linkService.getToolsLinks()
                        .pipe(
                            takeUntil(this._destroy),
                            tap((links) => {
                                this.toolsLinks = links;
                            })
                        );
                })
            )
            .subscribe();

        this._linkService.refreshCache()
            .subscribe();
    }

    public createColumn(): void {
        this._linkService.createColumn()
            .pipe(takeUntil(this._destroy))
            .subscribe();
    }

    public toggleWidgets(): void {
        this.showWidgets.next(!this.showWidgets.value);
    }

    public ngOnDestroy(): void {
        this._statService.ngOnDestroy();

        this._destroy.next();
    }

    public drop(targetList: Array<ILink>, $event: CdkDragDrop<Array<string>>): void {

        const item = $event.item.data;

        if (item.category === 'media') {
            this.mediaLinks.splice($event.previousIndex, 1);
        }

        if (item.category === 'productivity') {
            this.productivityLinks.splice($event.previousIndex, 1);
        }

        if (item.category === 'system') {
            this.systemLinks.splice($event.previousIndex, 1);
        }

        if (item.category === 'tools') {
            this.toolsLinks.splice($event.previousIndex, 1);
        }

        item.category = targetList[0].category;

        targetList.splice($event.currentIndex, 0, item);

        console.log(targetList, item);
    }

    public persistChanges(): void {

        this.isEditModeEnabled = !this.isEditModeEnabled;

        if (this.isEditModeEnabled) {
            return;
        }

        const sortedMediaLinks = this.mediaLinks.map((link, index) => {
            link.sortOrder = index;
            return link;
        });

        const sortedProductivityLinks = this.productivityLinks.map((link, index) => {
            link.sortOrder = index;
            return link;
        });

        const sortedSystemLinks = this.systemLinks.map((link, index) => {
            link.sortOrder = index;
            return link;
        });

        const sortedToolsLinks = this.toolsLinks.map((link, index) => {
            link.sortOrder = index;
            return link;
        });

        this._linkService.importLinks([
            ...sortedMediaLinks,
            ...sortedProductivityLinks,
            ...sortedSystemLinks,
            ...sortedToolsLinks
        ]).subscribe(() => {
            this.refreshLinkCache();
        });
    }
}
