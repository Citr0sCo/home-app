import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { StatService } from '../../services/stats-service/stat.service';
import { BuildService } from '../../services/build-service/build.service';
import {
    IDockerAppUpdateProgressResponse
} from '../../services/stats-service/types/docker-app-update-progress-response.response';
import { TerminalParser } from '../../core/terminal-parser';

@Component({
    selector: 'update-docker-apps-page',
    templateUrl: './update-docker-apps-page.component.html',
    styleUrls: ['./update-docker-apps-page.component.scss']
})
export class UpdateDockerAppsPageComponent implements OnInit, OnDestroy {

    public updateAllDockerAppsResult: IDockerAppUpdateProgressResponse = { finished: true, result: '' };
    public showLog: boolean = false;

    private readonly _subscriptions: Subscription = new Subscription();
    private readonly _statService: StatService;
    private readonly _buildService: BuildService;

    constructor(statService: StatService, buildService: BuildService) {
        this._statService = statService;
        this._buildService = buildService;
    }

    public ngOnInit(): void {
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
    }

    public updateAllDockerApps(): void {

        if (!this.updateAllDockerAppsResult.finished) {
            return;
        }

        this._buildService.updateAllDockerApps()
            .subscribe();
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }
}
