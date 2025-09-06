import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import {IReadarrActivity, IReadarrHealth} from "../../../../services/readarr-service/types/readarr-activity.type";
import {ReadarrService} from "../../../../services/readarr-service/readarr.service";

@Component({
    selector: 'readarr-details',
    templateUrl: './readarr-details.component.html',
    styleUrls: ['./readarr-details.component.scss'],
    standalone: false
})
export class ReadarrDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    public activity: IReadarrActivity | null = null;
    public readonly Object = Object;
    public groupedHealth: any | null = null;

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _readarrService: ReadarrService;

    constructor(lidarrService: ReadarrService) {
        this._readarrService = lidarrService;
    }

    public ngOnInit() {
        this._readarrService.getActivity()
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: IReadarrActivity) => {
                this.activity = activity;
                // @ts-ignore
                this.groupedHealth = Object.groupBy(this.activity.health, (x: any) => x.type);
            });

        this._readarrService.activity
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: IReadarrActivity) => {
                this.activity = activity;
                // @ts-ignore
                this.groupedHealth = Object.groupBy(this.activity.health, (x: any) => x.type);
            });

        this._readarrService.ngOnInit();
    }

    public getNumberOfType(healthType: string): number {
        return this.activity?.health?.filter((x) => x.type === healthType)?.length ?? 0;
    }

    public getTitle(problems: Array<IReadarrHealth>): string {

        let message = '';

        for (const problem of problems) {
            message += `${problem.message}\n\n`;
        }

        return message.slice(0, message.length - 2);
    }

    public ngOnDestroy(): void {
        this._readarrService.ngOnDestroy();

        this._destroy.next();
    }
}
