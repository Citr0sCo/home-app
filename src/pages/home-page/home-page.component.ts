import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { DeployService } from '../../services/deploy-service/deploy.service';
import { IDeploy } from '../../services/deploy-service/types/deploy.type';
import { BuildService } from '../../services/build-service/build.service';
import { IBuild } from '../../services/build-service/types/build.type';

@Component({
    selector: 'home-page',
    templateUrl: './home-page.component.html',
    styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit, OnDestroy {

    public deploys: Array<IDeploy> = [];
    public lastDeploy: IDeploy | null = null;
    public lastBuild: IBuild | null = null;
    public builds: Array<IBuild> = [];

    private readonly _deployService: DeployService;
    private readonly _buildService: BuildService;
    private readonly _destroy: Subject<void> = new Subject();

    constructor(deployService: DeployService, buildService: BuildService) {
        this._deployService = deployService;
        this._buildService = buildService;
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

        this._buildService.ngOnInit();
        this._deployService.ngOnInit();
    }

    public ngOnDestroy(): void {
        this._buildService.ngOnDestroy();
        this._deployService.ngOnDestroy();

        this._destroy.next();
    }
}
