import { Component, Input, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import { TautulliService } from '../../../../services/tautulli-service/tautulli.service';
import { ITautulliStats } from '../../../../services/tautulli-service/types/tautulli-stats.type';

@Component({
    selector: 'tautulli-details',
    templateUrl: './tautulli-details.component.html',
    styleUrls: ['./tautulli-details.component.scss'],
    standalone: false
})
export class TautulliDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    public stats: WritableSignal<ITautulliStats | null> = signal<ITautulliStats | null>(null);
    public isLoading: WritableSignal<boolean> = signal<boolean>(true);

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _tautulliService: TautulliService;

    constructor(tautulliService: TautulliService) {
        this._tautulliService = tautulliService;
    }

    public ngOnInit(): void {
        this._tautulliService.getStats(this.item?.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe({
                next: (stats: ITautulliStats) => {
                    this.stats.set(stats);
                    this.isLoading.set(false);
                },
                error: () => this.isLoading.set(false)
            });

        this._tautulliService.activities
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((activities: Array<ITautulliStats>) => {
                const stats = activities.find((activity) => activity.identifier === this.item?.identifier) ?? null;
                this.stats.set(stats);
                this.isLoading.set(false);
            });

        this._tautulliService.ngOnInit();
    }

    public ngOnDestroy(): void {
        this._tautulliService.ngOnDestroy();
        this._destroy.next();
        this._destroy.complete();
    }
}
