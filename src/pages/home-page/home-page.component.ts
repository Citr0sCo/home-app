import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription, switchMap, tap } from 'rxjs';
import { LinkService } from '../../services/link-service/link.service';
import { ILink } from '../../services/link-service/types/link.type';
import { DeployService } from '../../services/deploy-service/deploy.service';
import { IDeploy } from '../../services/deploy-service/types/deploy.type';
import { IStatResponse } from '../../services/stats-service/types/stat.response';
import { StatService } from '../../services/stats-service/stat.service';
import { PlexService } from '../../services/plex-service/plex.service';
import { BuildService } from '../../services/build-service/build.service';
import { IBuild } from '../../services/build-service/types/build.type';
import { WebSocketService } from '../../services/websocket-service/web-socket.service';
import { WebSocketKey } from '../../services/websocket-service/types/web-socket.key';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'home-page',
    templateUrl: './home-page.component.html',
    styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit, OnDestroy {

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
    public isConnected: boolean = false;
    public updateAllDockerAppsResult: string = '';
    public allStats: Array<IStatModel> = new Array<IStatModel>();

    private readonly _subscriptions: Subscription = new Subscription();
    private readonly _linkService: LinkService;
    private readonly _deployService: DeployService;
    private readonly _statService: StatService;
    private readonly _plexService: PlexService;
    private readonly _buildService: BuildService;
    private readonly _webSocketService: WebSocketService;

    constructor(linkService: LinkService, deployService: DeployService, statService: StatService, plexService: PlexService, buildService: BuildService) {
        this._linkService = linkService;
        this._deployService = deployService;
        this._statService = statService;
        this._plexService = plexService;
        this._buildService = buildService;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._subscriptions.add(
            this._deployService.getAll()
                .subscribe((response: Array<IDeploy>) => {
                    this.deploys = response;

                    this.deploys = this.deploys.sort((a, b) => {
                        return b.startedAt.getTime() - a.startedAt.getTime();
                    });

                    this.lastDeploy = this.deploys[0];
                })
        );

        this._subscriptions.add(
            this._deployService.deploys
                .asObservable()
                .subscribe((response: Array<IDeploy>) => {
                    this.deploys = response;

                    this.deploys = this.deploys.sort((a, b) => {
                        return b.startedAt.getTime() - a.startedAt.getTime();
                    });

                    this.lastDeploy = this.deploys[0];
                })
        );

        this._subscriptions.add(
            this._buildService.getAll()
                .subscribe((response) => {
                    this.builds = response;

                    this.builds = this.builds.sort((a, b) => {
                        return b.startedAt.getTime() - a.startedAt.getTime();
                    });

                    this.lastBuild = this.builds[0];
                })
        );

        this._subscriptions.add(
            this._buildService.builds
                .asObservable()
                .subscribe((response: Array<IBuild>) => {
                    this.builds = response;

                    this.builds = this.builds.sort((a, b) => {
                        return b.startedAt.getTime() - a.startedAt.getTime();
                    });

                    this.lastBuild = this.builds[0];
                })
        );

        this._subscriptions.add(
            this._statService.getAll()
                .subscribe((response: IStatResponse | null) => {
                    this.allStats = response?.stats ?? new Array<IStatModel>();
                })
        );

        this._subscriptions.add(
            this._statService.stats
                .asObservable()
                .subscribe((response: IStatResponse | null) => {
                    this.allStats = response?.stats ?? new Array<IStatModel>();
                })
        );

        this._subscriptions.add(
            this._statService.dockerAppUpdateProgress
                .asObservable()
                .subscribe((response: string | null) => {
                    this.updateAllDockerAppsResult = response ?? '';
                })
        );

        this._subscriptions.add(
            this._webSocketService.isConnected
                .asObservable()
                .subscribe((isConnected: boolean) => {
                    this.isConnected = isConnected;
                })
        );

        this._subscriptions.add(
            this._linkService.getAllLinks()
                .pipe(
                    switchMap(() => {
                        return this._linkService.getMediaLinks()
                            .pipe(
                                tap((links) => {
                                    this.mediaLinks = links;
                                })
                            );
                    }),
                    switchMap(() => {
                        return this._linkService.getSystemLinks()
                            .pipe(
                                tap((links) => {
                                    this.systemLinks = links;
                                })
                            );
                    }),
                    switchMap(() => {
                        return this._linkService.getProductivityLinks()
                            .pipe(
                                tap((links) => {
                                    this.productivityLinks = links;
                                })
                            );
                    }),
                    switchMap(() => {
                        return this._linkService.getToolsLinks()
                            .pipe(
                                tap((links) => {
                                    this.toolsLinks = links;
                                })
                            );
                    })
                )
                .subscribe()
        );

        setInterval(() => {
            this.currentTime = new Date();
        }, 1000);

        this._webSocketService.send(WebSocketKey.Handshake, { Test: 'Hello World!' });

        this._buildService.ngOnInit();
        this._deployService.ngOnInit();
        this._statService.ngOnInit();
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

    public getLastSortOrder(links: Array<ILink>): string {

        if (links.length === 0) {
            return 'A';
        }

        const alphabet = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'Z'];

        const lastItemSortOrder = links[links.length - 1].sortOrder;
        const sortOrderCharacters = lastItemSortOrder.split('');
        const lastLetter = sortOrderCharacters[sortOrderCharacters.length - 1];

        const letterOfAlphabetIndex = alphabet.indexOf(lastLetter);

        if (letterOfAlphabetIndex === (alphabet.length - 1)) {
            return `${lastItemSortOrder}A`;
        }

        return lastItemSortOrder + alphabet[letterOfAlphabetIndex + 1];
    }

    public searchWeb(): void {
        window.location.href = `https://www.google.com/search?q=${this.webQuery}`;
    }

    public refreshLinkCache(): void {
        this._linkService.refreshCache()
            .pipe(
                switchMap(() => {
                    return this._linkService.getMediaLinks()
                        .pipe(
                            tap((links) => {
                                this.mediaLinks = links;
                            })
                        );
                }),
                switchMap(() => {
                    return this._linkService.getSystemLinks()
                        .pipe(
                            tap((links) => {
                                this.systemLinks = links;
                            })
                        );
                }),
                switchMap(() => {
                    return this._linkService.getProductivityLinks()
                        .pipe(
                            tap((links) => {
                                this.productivityLinks = links;
                            })
                        );
                }),
                switchMap(() => {
                    return this._linkService.getToolsLinks()
                        .pipe(
                            tap((links) => {
                                this.toolsLinks = links;
                            })
                        );
                })
            )
            .subscribe();
    }

    public updateAllDockerApps(): void {
        this._buildService.updateAllDockerApps()
            .subscribe((result: string) => {
                this.updateAllDockerAppsResult = result;
            });
    }

    public ngOnDestroy(): void {
        this._buildService.ngOnDestroy();
        this._deployService.ngOnDestroy();
        this._statService.ngOnDestroy();

        this._subscriptions.unsubscribe();
    }
}
