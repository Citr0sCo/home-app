import { Component, Input, OnChanges, signal, WritableSignal } from '@angular/core';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'resource-monitor',
    templateUrl: './resource-monitor.component.html',
    styleUrls: ['./resource-monitor.component.scss'],
    standalone: false
})
export class ResourceMonitorComponent implements OnChanges {

    @Input()
    public allStats: WritableSignal<Array<IStatModel>> = signal<Array<IStatModel>>(new Array<IStatModel>());

    public stats: WritableSignal<IStatModel | null> = signal<IStatModel | null>(null);

    public ngOnChanges(): void {
        const statsArray = this.allStats();

        // Handle case where allStats might be undefined or empty
        if (!statsArray || statsArray.length === 0) {
            this.stats.set(null);
            return;
        }

        const homeAppStats = statsArray.find((x) => x.name && x.name.indexOf('home-app') > -1);

        // Handle case where homeAppStats is not found
        if (!homeAppStats) {
            // Fallback to total usage across all apps
            this.stats.set({
                cpuUsage: {
                    percentage: statsArray.map((y) => y.cpuUsage).reduce((y, { percentage }) => y + percentage, 0),
                    total: 0,
                    used: statsArray.map((y) => y.cpuUsage).reduce((y, { used }) => y + used, 0)
                },
                memoryUsage: {
                    percentage: statsArray.map((y) => y.memoryUsage).reduce((y, { percentage }) => y + percentage, 0),
                    total: 0,
                    used: statsArray.map((y) => y.memoryUsage).reduce((y, { used }) => y + used, 0)
                },
                diskUsage: {
                    percentage: 0,
                    used: 0,
                    total: 0
                },
                name: 'app'
            });
            return;
        }

        this.stats.set({
            cpuUsage: {
                percentage: statsArray.map((y) => y.cpuUsage).reduce((y, { percentage }) => y + percentage, 0),
                total: homeAppStats?.cpuUsage.total ?? 0,
                used: homeAppStats?.cpuUsage.used ?? 0
            },
            memoryUsage: {
                percentage: statsArray.map((y) => y.memoryUsage).reduce((y, { percentage }) => y + percentage, 0),
                total: homeAppStats?.memoryUsage.total ?? 0,
                used: statsArray.map((y) => y.memoryUsage).reduce((y, { used }) => y + used, 0)
            },
            diskUsage: homeAppStats?.diskUsage ?? {
                percentage: 0,
                used: 0,
                total: 0
            },
            name: homeAppStats?.name ?? 'app'
        });
    }

    public bytesToGigaBytes(valueInBytes: number): number {
        return Math.round((valueInBytes / 1000000000) * 100) / 100;
    }

    public roundToTwoDecmalPoints(valueInBytes: number): number {
        return Math.round((valueInBytes) * 100) / 100;
    }
}
