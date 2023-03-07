import { Component, Input, OnInit } from '@angular/core';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'resource-monitor',
    templateUrl: './resource-monitor.component.html',
    styleUrls: ['./resource-monitor.component.scss']
})
export class ResourceMonitorComponent implements OnInit {

    @Input()
    public allStats: Array<IStatModel> = new Array<IStatModel>();

    public stats: IStatModel | null = null;

    public ngOnInit(): void {
        this.stats = this.allStats.find((x) => x.name === 'home-app') ?? null;
    }

    public bytesToGigaBytes(valueInBytes: number): number {
        return Math.round((valueInBytes / 1000000000) * 100) / 100;
    }
}
