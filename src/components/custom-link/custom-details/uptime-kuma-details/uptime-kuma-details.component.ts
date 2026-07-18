import { Component, Input, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import { UptimeKumaService } from '../../../../services/uptime-kuma-service/uptime-kuma.service';
import {
    IUptimeKumaActivity,
    IUptimeKumaMetric
} from '../../../../services/uptime-kuma-service/types/uptime-kuma-activity.type';

@Component({
    selector: 'uptime-kuma-details',
    templateUrl: './uptime-kuma-details.component.html',
    styleUrls: ['./uptime-kuma-details.component.scss'],
    standalone: false
})
export class UptimeKumaDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    public activity: WritableSignal<IUptimeKumaActivity | null> = signal<IUptimeKumaActivity | null>(null);

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _uptimeKumaService: UptimeKumaService;

    constructor(uptimeKumaService: UptimeKumaService) {
        this._uptimeKumaService = uptimeKumaService;
    }

    public ngOnInit() {
        this._uptimeKumaService.getActivity(this.item?.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe((activity: IUptimeKumaActivity) => {
                this.activity.set(activity);
            });

        this._uptimeKumaService.activities
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((response: Array<IUptimeKumaActivity>) => {
                this.activity.set(response.length > 0 ? response[0] : null);
            });

        this._uptimeKumaService.ngOnInit();
    }

    public ngOnDestroy(): void {
        this._uptimeKumaService.ngOnDestroy();

        this._destroy.next();
    }

    protected publicServices(): IUptimeKumaMetric | null {
        return this.activity()?.metrics?.find((x) => x.name === 'Public Services') ?? null;
    }

    protected localServices(): IUptimeKumaMetric | null {
        return this.activity()?.metrics?.find((x) => x.name === 'Local Services') ?? null;
    }

    protected servers(): IUptimeKumaMetric | null {
        return this.activity()?.metrics?.find((x) => x.name === 'Servers') ?? null;
    }
}
