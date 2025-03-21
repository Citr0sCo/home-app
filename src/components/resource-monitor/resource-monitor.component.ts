import { Component, Input, OnChanges } from '@angular/core';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'resource-monitor',
    templateUrl: './resource-monitor.component.html',
    styleUrls: ['./resource-monitor.component.scss']
})
export class ResourceMonitorComponent implements OnChanges {

    @Input()
    public allStats: Array<IStatModel> = new Array<IStatModel>();

    public stats: IStatModel | null = null;

    public ngOnChanges(): void {
        this.stats = this.allStats.find((x) => x.name.indexOf('home-app') > -1) ?? null;
    }

    public bytesToGigaBytes(valueInBytes: number): number {
        return Math.round((valueInBytes / 1000000000) * 100) / 100;
    }
}
