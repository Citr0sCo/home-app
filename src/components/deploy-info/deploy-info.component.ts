import { Component, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subscription } from 'rxjs';
import { IStatResponse } from '../../services/stats-service/types/stat.response';
import { StatService } from '../../services/stats-service/stat.service';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'deploy-info',
    templateUrl: './deploy-info.component.html',
    styleUrls: ['./deploy-info.component.scss'],
    standalone: false
})
export class DeployInfoComponent implements OnInit, OnDestroy {

    public allStats: WritableSignal<Array<IStatModel>> = signal<Array<IStatModel>>(new Array<IStatModel>());

    private readonly _subscriptions: Subscription = new Subscription();
    private readonly _statService: StatService;

    constructor(statService: StatService) {
        this._statService = statService;
    }

    public ngOnInit(): void {
        this._subscriptions.add(
            this._statService.getAll()
                .subscribe((response: IStatResponse | null) => {
                    this.allStats.set(response?.stats ?? new Array<IStatModel>());
                    console.log(this.allStats());
                })
        );

        this._subscriptions.add(
            this._statService.stats
                .asObservable()
                .subscribe((response: IStatResponse | null) => {
                    this.allStats.set(response?.stats ?? new Array<IStatModel>());
                    console.log(this.allStats());
                })
        );

        this._statService.ngOnInit();
    }

    public ngOnDestroy(): void {
        this._statService.ngOnDestroy();

        this._subscriptions.unsubscribe();
    }
}
