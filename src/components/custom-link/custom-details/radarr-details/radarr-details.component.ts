import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import { RadarrService } from '../../../../services/radarr-service/radarr.service';
import { IRadarrActivity } from '../../../../services/radarr-service/types/radarr-activity.type';

@Component({
    selector: 'radarr-details',
    templateUrl: './radarr-details.component.html',
    styleUrls: ['./radarr-details.component.scss']
})
export class RadarrDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    public activity: IRadarrActivity | null = null;

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _radarrService: RadarrService;

    constructor(radarrService: RadarrService) {
        this._radarrService = radarrService;
    }

    public ngOnInit() {
        this._radarrService.getActivity(this.item?.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: IRadarrActivity) => {
                this.activity = activity;
            });

        this._radarrService.activity
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: IRadarrActivity) => {
                this.activity = activity;
            });

        this._radarrService.ngOnInit();
    }

    public ngOnDestroy(): void {
        this._radarrService.ngOnDestroy();

        this._destroy.next();
    }
}
