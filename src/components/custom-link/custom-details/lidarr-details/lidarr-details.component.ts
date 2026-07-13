import {Component, Input, OnDestroy, OnInit, signal, WritableSignal} from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import {ILidarrActivity, ILidarrHealth} from "../../../../services/lidarr-service/types/lidarr-activity.type";
import {LidarrService} from "../../../../services/lidarr-service/lidarr.service";

@Component({
    selector: 'lidarr-details',
    templateUrl: './lidarr-details.component.html',
    styleUrls: ['./lidarr-details.component.scss'],
    standalone: false
})
export class LidarrDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: WritableSignal<ILink | null> = signal<ILink | null>(null);

    public activity: WritableSignal<ILidarrActivity | null> = signal<ILidarrActivity | null>(null);
    public readonly Object = Object;
    public groupedHealth: any | null = null;

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _lidarrService: LidarrService;

    constructor(lidarrService: LidarrService) {
        this._lidarrService = lidarrService;
    }

    public ngOnInit() {
        this._lidarrService.getActivity()
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: ILidarrActivity) => {
                this.activity.set(activity);
                // @ts-ignore
                this.groupedHealth = Object.groupBy(this.activity()!.health, (x: any) => x.type);
            });

        this._lidarrService.activity
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: ILidarrActivity) => {
                this.activity.set(activity);
                // @ts-ignore
                this.groupedHealth = Object.groupBy(this.activity()!.health, (x: any) => x.type);
            });

        this._lidarrService.ngOnInit();
    }

    public getNumberOfType(healthType: string): number {
        return this.activity()?.health?.filter((x) => x.type === healthType)?.length ?? 0;
    }

    public getTitle(problems: Array<ILidarrHealth>): string {

        let message = '';

        for (const problem of problems) {
            message += `${problem.message}\n\n`;
        }

        return message.slice(0, message.length - 2);
    }

    public ngOnDestroy(): void {
        this._lidarrService.ngOnDestroy();

        this._destroy.next();
    }
}
