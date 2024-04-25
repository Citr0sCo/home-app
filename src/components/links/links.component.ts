import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subject, Subscription, switchMap, takeUntil, tap } from 'rxjs';
import { LinkService } from '../../services/link-service/link.service';
import { ILink } from '../../services/link-service/types/link.type';
import { DeployService } from '../../services/deploy-service/deploy.service';
import { IDeploy } from '../../services/deploy-service/types/deploy.type';
import { IStatResponse } from '../../services/stats-service/types/stat.response';
import { StatService } from '../../services/stats-service/stat.service';
import { BuildService } from '../../services/build-service/build.service';
import { IBuild } from '../../services/build-service/types/build.type';
import { WebSocketService } from '../../services/websocket-service/web-socket.service';
import { WebSocketKey } from '../../services/websocket-service/types/web-socket.key';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'links',
    templateUrl: './links.component.html',
    styleUrls: ['./links.component.scss']
})
export class LinksComponent implements OnInit, OnDestroy {

    public mediaLinks: Array<ILink> = [];
    public systemLinks: Array<ILink> = [];
    public productivityLinks: Array<ILink> = [];
    public toolsLinks: Array<ILink> = [];
    public currentTime: Date = new Date();
    public deploys: Array<IDeploy> = [];
    public lastDeploy: IDeploy | null = null;
    public lastBuild: IBuild | null = null;
    public builds: Array<IBuild> = [];
    public isEditModeEnabled: boolean = false;
    public webQuery: string = '';
    public allStats: Array<IStatModel> = new Array<IStatModel>();
    public refreshCache: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    private readonly _linkService: LinkService;
    private readonly _deployService: DeployService;
    private readonly _statService: StatService;
    private readonly _buildService: BuildService;
    private readonly _webSocketService: WebSocketService;
    private readonly _destroy: Subject<void> = new Subject();

    constructor(linkService: LinkService, deployService: DeployService, statService: StatService, buildService: BuildService) {
        this._linkService = linkService;
        this._deployService = deployService;
        this._statService = statService;
        this._buildService = buildService;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {

        this._deployService.getAll()
            .pipe(takeUntil(this._destroy))
            .subscribe((response: Array<IDeploy>) => {
                this.deploys = response;

                this.deploys = this.deploys.sort((a, b) => {
                    return b.startedAt.getTime() - a.startedAt.getTime();
                });

                this.lastDeploy = this.deploys[0];
            });

        this._deployService.deploys
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((response: Array<IDeploy>) => {
                this.deploys = response;

                this.deploys = this.deploys.sort((a, b) => {
                    return b.startedAt.getTime() - a.startedAt.getTime();
                });

                this.lastDeploy = this.deploys[0];
            });

        this._buildService.getAll()
            .pipe(takeUntil(this._destroy))
            .subscribe((response) => {
                this.builds = response;

                this.builds = this.builds.sort((a, b) => {
                    return b.startedAt.getTime() - a.startedAt.getTime();
                });

                this.lastBuild = this.builds[0];
            });

        this._buildService.builds
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((response: Array<IBuild>) => {
                this.builds = response;

                this.builds = this.builds.sort((a, b) => {
                    return b.startedAt.getTime() - a.startedAt.getTime();
                });

                this.lastBuild = this.builds[0];
            });

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

        setInterval(() => {
            this.currentTime = new Date();
        }, 1000);

        this._webSocketService.send(WebSocketKey.Handshake, { Test: 'Hello World!' });

        this._buildService.ngOnInit();
        this._deployService.ngOnInit();
        this._statService.ngOnInit();
    }

    public getLastSortOrder(links: Array<ILink>): string {

        if (links.length === 0) {
            return 'A';
        }

        const alphabet = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'Z'];

        const lastItemSortOrder = links[links.length - 1].sortOrder;
        const sortOrderCharacters = lastItemSortOrder.split('');
        const lastLetter = sortOrderCharacters[sortOrderCharacters.length - 1];

        const lastLetterOfLastTimeIndex = alphabet.indexOf(lastLetter);

        if (lastLetterOfLastTimeIndex === (alphabet.length - 1)) {
            return `${lastItemSortOrder}A`;
        }

        const lastIndexWithLastCharacter = lastItemSortOrder.slice(0, -1);

        return lastIndexWithLastCharacter + alphabet[lastLetterOfLastTimeIndex + 1];
    }

    public searchWeb(): void {
        window.location.href = `https://www.google.com/search?q=${this.webQuery}`;
    }

    public refreshLinkCache(): void {

        this.refreshCache.next(true);

        this._linkService.refreshCache()
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
    }

    public createColumn(): void {
        this._linkService.createColumn()
            .pipe(takeUntil(this._destroy))
            .subscribe();
    }

    public ngOnDestroy(): void {
        this._buildService.ngOnDestroy();
        this._deployService.ngOnDestroy();
        this._statService.ngOnDestroy();

        this._destroy.next();
    }
}
