import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { DeployService } from '../../services/deploy-service/deploy.service';
import { IDeploy } from '../../services/deploy-service/types/deploy.type';
import { IStatResponse } from '../../services/stats-service/types/stat.response';
import { StatService } from '../../services/stats-service/stat.service';
import { BuildService } from '../../services/build-service/build.service';
import { IBuild } from '../../services/build-service/types/build.type';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'deploy-info',
    templateUrl: './deploy-info.component.html',
    styleUrls: ['./deploy-info.component.scss']
})
export class DeployInfoComponent implements OnInit, OnDestroy {

    public deploys: Array<IDeploy> = [];
    public lastDeploy: IDeploy | null = null;
    public lastBuild: IBuild | null = null;
    public builds: Array<IBuild> = [];
    public allStats: Array<IStatModel> = new Array<IStatModel>();

    private readonly _subscriptions: Subscription = new Subscription();
    private readonly _deployService: DeployService;
    private readonly _statService: StatService;
    private readonly _buildService: BuildService;

    constructor(deployService: DeployService, statService: StatService, buildService: BuildService) {
        this._deployService = deployService;
        this._statService = statService;
        this._buildService = buildService;
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

        this._buildService.ngOnInit();
        this._deployService.ngOnInit();
        this._statService.ngOnInit();
    }

    public ngOnDestroy(): void {
        this._buildService.ngOnDestroy();
        this._deployService.ngOnDestroy();
        this._statService.ngOnDestroy();

        this._subscriptions.unsubscribe();
    }
}
