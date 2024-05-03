import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import { SonarrService } from '../../../../services/sonarr-service/sonarr.service';
import { ISonarrActivity } from '../../../../services/sonarr-service/types/sonarr-activity.type';

@Component({
    selector: 'sonarr-details',
    templateUrl: './sonarr-details.component.html',
    styleUrls: ['./sonarr-details.component.scss']
})
export class SonarrDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    public activity: ISonarrActivity | null = null;

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _sonarrService: SonarrService;

    constructor(sonarrService: SonarrService) {
        this._sonarrService = sonarrService;
    }

    public ngOnInit() {
        this._sonarrService.getActivity(this.item?.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: ISonarrActivity) => {
                this.activity = activity;
            });

        this._sonarrService.activity
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: ISonarrActivity) => {
                this.activity = activity;
            });

        this._sonarrService.ngOnInit();
    }

    public ngOnDestroy(): void {
        this._sonarrService.ngOnDestroy();

        this._destroy.next();
    }
}
