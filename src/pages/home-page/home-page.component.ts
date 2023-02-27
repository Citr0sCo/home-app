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
import { BuildConclusion } from '../../services/build-service/types/build-conclusion.enum';
import { BuildStatus } from '../../services/build-service/types/build-status.enum';

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
    public stats: IStatResponse | null = null;
    public builds: Array<IBuild> = [];
    public buildConclusion: typeof BuildConclusion = BuildConclusion;
    public buildStatus: typeof BuildStatus = BuildStatus;
    public isEditing: boolean = false;
    public webQuery: string = '';

    private readonly _subscriptions: Subscription = new Subscription();
    private readonly _linkService: LinkService;
    private readonly _deployService: DeployService;
    private readonly _statService: StatService;
    private readonly _plexService: PlexService;
    private readonly _buildService: BuildService;

    constructor(linkService: LinkService, deployService: DeployService, statService: StatService, plexService: PlexService, buildService: BuildService) {
        this._linkService = linkService;
        this._deployService = deployService;
        this._statService = statService;
        this._plexService = plexService;
        this._buildService = buildService;
    }

    public ngOnInit(): void {
        this._subscriptions.add(
            this._deployService.getAll()
                .subscribe((response) => {
                    this.deploys = response;

                    this.deploys = this.deploys.sort((a, b) => {
                        return b.startedAt.getTime() - a.startedAt.getTime();
                    });

                    this.lastDeploy = this.deploys[0];
                })
        );

        this._subscriptions.add(
            this._statService.getAll()
                .subscribe((response) => {
                    this.stats = response;
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

        setInterval(() => {
            this._statService.getAll()
                .subscribe((response) => {
                    this.stats = response;
                });
        }, 5000);
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

    public bytesToGigaBytes(valueInBytes: number): number {
        return Math.round((valueInBytes / 1000000000) * 100) / 100;
    }

    public getPercentageFor(percentage: number): number {
        return Math.round((percentage * 100) * 100) / 100;
    }

    public getLastSortOrder(links: Array<ILink>): number {

        if (links.length === 0) {
            return 0;
        }

        return links[links.length - 1].sortOrder + 1;
    }

    public searchWeb(): void {
        window.location.href = `https://www.google.com/search?q=${this.webQuery}`;
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }
}
