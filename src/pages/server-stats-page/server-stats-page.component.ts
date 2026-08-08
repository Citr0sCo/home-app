import { Component, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { StatService } from '../../services/stats-service/stat.service';
import { IStatHistoryResponse, IStatHistorySample } from '../../services/stats-service/types/stat-history.response';

type Metric = 'cpuPercentage' | 'memoryPercentage' | 'diskPercentage';

interface ChartDefinition {
    key: Metric;
    label: string;
    icon: string;
    color: string;
    description: string;
}

@Component({
    selector: 'server-stats-page',
    templateUrl: './server-stats-page.component.html',
    styleUrls: ['./server-stats-page.component.scss'],
    standalone: false
})
export class ServerStatsPageComponent implements OnInit, OnDestroy {
    public readonly charts: Array<ChartDefinition> = [
        {
            key: 'cpuPercentage',
            label: 'CPU usage',
            icon: 'fas fa-microchip',
            color: '#ff8930',
            description: 'Combined container load'
        },
        {
            key: 'memoryPercentage',
            label: 'Memory usage',
            icon: 'fas fa-memory',
            color: '#36a4ff',
            description: 'Memory used across containers'
        },
        {
            key: 'diskPercentage',
            label: 'Disk usage',
            icon: 'fas fa-hdd',
            color: '#2ecc71',
            description: 'Host storage used'
        }
    ];

    public history: WritableSignal<IStatHistoryResponse | null> = signal<IStatHistoryResponse | null>(null);
    public isLoading: WritableSignal<boolean> = signal<boolean>(true);
    public hasError: WritableSignal<boolean> = signal<boolean>(false);

    private readonly _statService: StatService;
    private readonly _destroy: Subject<void> = new Subject();

    constructor(statService: StatService) {
        this._statService = statService;
    }

    public ngOnInit(): void {
        this._statService.getHistory()
            .pipe(takeUntil(this._destroy))
            .subscribe({
                next: (history) => {
                    this.history.set(history);
                    this.hasError.set(history.hasError === true);
                    this.isLoading.set(false);
                },
                error: () => {
                    this.hasError.set(true);
                    this.isLoading.set(false);
                }
            });
    }

    public samples(): Array<IStatHistorySample> {
        return this.history()?.samples ?? [];
    }

    public valueFor(sample: IStatHistorySample, metric: Metric): number {
        return sample[metric] ?? 0;
    }

    public currentValue(metric: Metric): number {
        const samples = this.samples();
        return samples.length === 0 ? 0 : this.valueFor(samples[samples.length - 1], metric);
    }

    public minimumValue(metric: Metric): number {
        const samples = this.samples();
        return samples.length === 0 ? 0 : Math.min(...samples.map((sample) => this.valueFor(sample, metric)));
    }

    public maximumValue(metric: Metric): number {
        const samples = this.samples();
        return samples.length === 0 ? 0 : Math.max(...samples.map((sample) => this.valueFor(sample, metric)));
    }

    public chartLine(metric: Metric): string {
        return this.chartCoordinates(metric).map((point) => `${point.x},${point.y}`).join(' ');
    }

    public chartArea(metric: Metric): string {
        const coordinates = this.chartCoordinates(metric);
        if (coordinates.length === 0) {
            return '';
        }

        const first = coordinates[0];
        const last = coordinates[coordinates.length - 1];
        return `${first.x},204 ${this.chartLine(metric)} ${last.x},204`;
    }

    public chartPointX(index: number): number {
        const samples = this.samples();
        return samples.length <= 1 ? 24 : 24 + (index / (samples.length - 1)) * 672;
    }

    public chartPointY(sample: IStatHistorySample, metric: Metric): number {
        const value = Math.max(0, Math.min(100, this.valueFor(sample, metric)));
        return 204 - value * 1.8;
    }

    public formatPercentage(value: number): string {
        return `${value.toFixed(1)}%`;
    }

    public formatTimestamp(timestamp: string): string {
        return new Date(timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    }

    public rangeLabel(): string {
        const history = this.history();
        if (!history) {
            return 'Last 24 hours';
        }

        return `${this.formatTimestamp(history.from)} — ${this.formatTimestamp(history.to)}`;
    }

    public ngOnDestroy(): void {
        this._destroy.next();
        this._destroy.complete();
    }

    private chartCoordinates(metric: Metric): Array<{ x: number; y: number }> {
        const samples = this.samples();
        const lastIndex = samples.length - 1;

        return samples.map((sample, index) => ({
            x: lastIndex <= 0 ? 24 : 24 + (index / lastIndex) * 672,
            y: this.chartPointY(sample, metric)
        }));
    }
}
