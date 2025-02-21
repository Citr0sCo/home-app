import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import { IPiholeActivity } from '../../../../services/pihole-service/types/pihole-activity.type';
import { PiholeService } from '../../../../services/pihole-service/pihole.service';

@Component({
    selector: 'pihole-details',
    templateUrl: './pihole-details.component.html',
    styleUrls: ['./pihole-details.component.scss']
})
export class PiholeDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    public activity: IPiholeActivity | null = null;

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _piholeService: PiholeService;

    constructor(piholeService: PiholeService) {
        this._piholeService = piholeService;
    }

    public ngOnInit() {
        this._piholeService.getActivity(this.item?.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: IPiholeActivity) => {
                this.activity = activity;
            });

        this._piholeService.activities
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((response: Array<IPiholeActivity>) => {
                this.activity = response.find((x) => x.identifier === this.item?.identifier) ?? null;
            });

        this._piholeService.ngOnInit();
    }

    public ngOnDestroy(): void {
        this._piholeService.ngOnDestroy();

        this._destroy.next();
    }
}
