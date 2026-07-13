import {Component, Input, OnDestroy, OnInit, signal, WritableSignal} from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import { RadarrService } from '../../../../services/radarr-service/radarr.service';
import {IRadarrActivity, IRadarrHealth} from '../../../../services/radarr-service/types/radarr-activity.type';

@Component({
    selector: 'radarr-details',
    templateUrl: './radarr-details.component.html',
    styleUrls: ['./radarr-details.component.scss'],
    standalone: false
})
export class RadarrDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: WritableSignal<ILink | null> = signal<ILink | null>(null);

    public activity: WritableSignal<IRadarrActivity | null> = signal<IRadarrActivity | null>(null);
    public readonly Object = Object;
    public groupedHealth: any | null = null;

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _radarrService: RadarrService;

    constructor(radarrService: RadarrService) {
        this._radarrService = radarrService;
    }

    public ngOnInit() {
        this._radarrService.getActivity()
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: IRadarrActivity) => {
                this.activity.set(activity);
                // @ts-ignore
                this.groupedHealth = Object.groupBy(this.activity()!.health, (x: any) => x.type);
            });

        this._radarrService.activity
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: IRadarrActivity) => {
                this.activity.set(activity);
                // @ts-ignore
                this.groupedHealth = Object.groupBy(this.activity()!.health, (x: any) => x.type);
            });

        this._radarrService.ngOnInit();
    }

    public getNumberOfType(healthType: string): number {
        return this.activity()?.health?.filter((x) => x.type === healthType)?.length ?? 0;
    }

    public getTitle(problems: Array<IRadarrHealth>): string {

        let message = '';

        for (const problem of problems) {
            message += `${problem.message}\n\n`;
        }

        return message.slice(0, message.length - 2);
    }

    public ngOnDestroy(): void {
        this._radarrService.ngOnDestroy();

        this._destroy.next();
    }
}
