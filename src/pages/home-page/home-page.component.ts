import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subscription, switchMap, tap } from 'rxjs';
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
import {
    IDockerAppUpdateProgressResponse
} from '../../services/stats-service/types/docker-app-update-progress-response.response';
import { TerminalParser } from '../../core/terminal-parser';

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
    public updateAllDockerAppsResult: IDockerAppUpdateProgressResponse = { finished: true, result: '' };
    public allStats: Array<IStatModel> = new Array<IStatModel>();
    public showLog: boolean = false;
    public refreshCache: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    private readonly _subscriptions: Subscription = new Subscription();
    private readonly _linkService: LinkService;
    private readonly _deployService: DeployService;
    private readonly _statService: StatService;
    private readonly _buildService: BuildService;
    private readonly _webSocketService: WebSocketService;

    constructor(linkService: LinkService, deployService: DeployService, statService: StatService, buildService: BuildService) {
        this._linkService = linkService;
        this._deployService = deployService;
        this._statService = statService;
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
                .subscribe((response: IDockerAppUpdateProgressResponse | null) => {

                    const parsedOutput = new TerminalParser(response!.result).toHtml();

                    if (parsedOutput.length === 0) {
                        this.updateAllDockerAppsResult.finished = true;
                        return;
                    }

                    this.updateAllDockerAppsResult = {
                        result: parsedOutput,
                        finished: response!.finished
                    };

                    this.showLog = true;
                    const logWindow = document.querySelector('.log-window');
                    logWindow?.scrollTo(0, logWindow!.scrollHeight + 500);
                })
        );

        this._subscriptions.add(
            this._webSocketService.isConnected
                .asObservable()
                .subscribe((isConnected: boolean) => {
                    this.isConnected = isConnected;

                    if (!this.isConnected) {
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

    public connectToWebSocket(): void {

        if (this.isConnected) {
            return;
        }

        this._webSocketService.connect();
    }

    public updateAllDockerApps(): void {

        if (!this.updateAllDockerAppsResult.finished) {
            return;
        }

        this._buildService.updateAllDockerApps()
            .subscribe();
    }

    public ngOnDestroy(): void {
        this._buildService.ngOnDestroy();
        this._deployService.ngOnDestroy();
        this._statService.ngOnDestroy();

        this._subscriptions.unsubscribe();
    }
}
