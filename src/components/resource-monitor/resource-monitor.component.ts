import { Component, computed, input, Signal } from '@angular/core';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'resource-monitor',
    templateUrl: './resource-monitor.component.html',
    styleUrls: ['./resource-monitor.component.scss'],
    standalone: false
})
export class ResourceMonitorComponent {

    public allStats = input<Array<IStatModel> | undefined>([]);

    public stats: Signal<IStatModel | null> = computed(() => this.aggregateStats(this.allStats()));

    public bytesToGigaBytes(valueInBytes: number): number {
        return Math.round((valueInBytes / 1000000000) * 100) / 100;
    }

    public roundToTwoDecmalPoints(valueInBytes: number): number {
        return Math.round((valueInBytes) * 100) / 100;
    }

    private aggregateStats(statsArray: Array<IStatModel> | undefined): IStatModel | null {
        const validStats = statsArray?.filter((stat) => stat !== null && stat !== undefined) ?? [];

        if (validStats.length === 0) {
            return null;
        }

        const memoryUsed = this.sum(validStats.map((stat) => stat.memoryUsage?.used));
        const memoryTotal = this.maximum(validStats.map((stat) => stat.memoryUsage?.total));
        const diskTotal = this.maximum(validStats.map((stat) => stat.diskUsage?.total));
        const diskUsed = this.maximum(validStats.map((stat) => stat.diskUsage?.used));

        return {
            cpuUsage: {
                percentage: Math.min(this.sum(validStats.map((stat) => stat.cpuUsage?.percentage)), 100),
                total: this.sum(validStats.map((stat) => stat.cpuUsage?.total)),
                used: this.sum(validStats.map((stat) => stat.cpuUsage?.used))
            },
            memoryUsage: {
                percentage: memoryTotal > 0
                    ? (memoryUsed / memoryTotal) * 100
                    : this.sum(validStats.map((stat) => stat.memoryUsage?.percentage)),
                total: memoryTotal,
                used: memoryUsed
            },
            diskUsage: {
                percentage: diskTotal > 0
                    ? (diskUsed / diskTotal) * 100
                    : this.maximum(validStats.map((stat) => stat.diskUsage?.percentage)),
                total: diskTotal,
                used: diskUsed
            },
            name: 'server'
        };
    }

    private sum(values: Array<number | undefined>): number {
        return values.reduce<number>((total, value) => total + (value ?? 0), 0);
    }

    private maximum(values: Array<number | undefined>): number {
        return values.reduce<number>((maximum, value) => Math.max(maximum, value ?? 0), 0);
    }
}
