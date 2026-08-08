import { Component, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { catchError, EMPTY, map, merge, Subject, switchMap, takeUntil, timer } from 'rxjs';
import { StatService } from '../../services/stats-service/stat.service';
import { IStatHistoryResponse, IStatHistorySample } from '../../services/stats-service/types/stat-history.response';

type Metric = 'cpuPercentage' | 'memoryPercentage' | 'diskPercentage';

interface ChartDefinition {
    key: Metric;
    label: string;
    icon: string;
    color: string;
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
            color: '#ff8930'
        },
        {
            key: 'memoryPercentage',
            label: 'Memory usage',
            icon: 'fas fa-memory',
            color: '#36a4ff'
        },
        {
            key: 'diskPercentage',
            label: 'Disk usage',
            icon: 'fas fa-hdd',
            color: '#2ecc71'
        }
    ];

    public readonly ranges = [
        { hours: 1, label: 'Last hour' },
        { hours: 6, label: 'Last 6 hours' },
        { hours: 12, label: 'Last 12 hours' },
        { hours: 24, label: 'Last 24 hours' },
        { hours: 24 * 7, label: 'Last 7 days' }
    ];

    public history: WritableSignal<IStatHistoryResponse | null> = signal<IStatHistoryResponse | null>(null);
    public hoveredPoint: WritableSignal<{ sample: IStatHistorySample; metric: Metric } | null> = signal(null);
    public selectedRangeHours: WritableSignal<number> = signal<number>(24);
    public isLoading: WritableSignal<boolean> = signal<boolean>(true);
    public hasError: WritableSignal<boolean> = signal<boolean>(false);

    private readonly _statService: StatService;
    private readonly _destroy: Subject<void> = new Subject();
    private readonly _rangeChanges: Subject<number> = new Subject();

    constructor(statService: StatService) {
        this._statService = statService;
    }

    public ngOnInit(): void {
        merge(
            timer(0, 15000).pipe(map(() => this.selectedRangeHours())),
            this._rangeChanges
        )
            .pipe(
                switchMap((hours) => this._statService.getHistory(hours).pipe(
                    catchError(() => {
                        this.hasError.set(true);
                        this.isLoading.set(false);
                        return EMPTY;
                    })
                )),
                takeUntil(this._destroy)
            )
            .subscribe({
                next: (history) => {
                    this.history.set(history);
                    this.hoveredPoint.set(null);
                    this.hasError.set(history.hasError === true);
                    this.isLoading.set(false);
                },
                error: () => {
                    this.hasError.set(true);
                    this.isLoading.set(false);
                }
            });
    }

    public onRangeChange(event: Event): void {
        const hours = Number((event.target as HTMLSelectElement).value);
        if (!this.ranges.some((range) => range.hours === hours)) {
            return;
        }

        this.selectedRangeHours.set(hours);
        this.hoveredPoint.set(null);
        this.isLoading.set(true);
        this.hasError.set(false);
        this._rangeChanges.next(hours);
    }

    public onChartHover(event: MouseEvent, metric: Metric): void {
        const samples = this.samples();
        const svg = (event.currentTarget as SVGRectElement).ownerSVGElement;
        if (samples.length === 0 || !svg) {
            return;
        }

        const bounds = svg.getBoundingClientRect();
        const chartX = 24 + ((event.clientX - bounds.left) / bounds.width) * 672;
        const sample = samples.reduce((closest, candidate) =>
            Math.abs(this.chartPointX(candidate) - chartX) < Math.abs(this.chartPointX(closest) - chartX)
                ? candidate
                : closest);

        this.hoveredPoint.set({ sample, metric });
    }

    public clearHoveredPoint(): void {
        this.hoveredPoint.set(null);
    }

    public hoveredPointX(): number {
        const point = this.hoveredPoint();
        return point ? Math.max(60, Math.min(660, this.chartPointX(point.sample))) : 0;
    }

    public hoveredPointY(): number {
        const point = this.hoveredPoint();
        return point ? Math.max(36, this.chartPointY(point.sample, point.metric) - 8) : 0;
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

    public chartPointX(sample: IStatHistorySample): number {
        const history = this.history();
        if (!history) {
            return 24;
        }

        const from = new Date(history.from).getTime();
        const to = new Date(history.to).getTime();
        const timestamp = new Date(sample.recordedAt).getTime();
        const duration = to - from;
        const progress = duration <= 0 ? 1 : Math.max(0, Math.min(1, (timestamp - from) / duration));

        return 24 + progress * 672;
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

    public formatAxisTimestamp(timestamp: string): string {
        const options: Intl.DateTimeFormatOptions = this.selectedRangeHours() > 24
            ? { month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' }
            : { hour: '2-digit', minute: '2-digit' };

        return new Date(timestamp).toLocaleString([], options);
    }

    public rangeLabel(): string {
        const range = this.ranges.find((item) => item.hours === this.selectedRangeHours());
        const history = this.history();
        if (!history) {
            return range?.label ?? 'Selected range';
        }

        return `${range?.label ?? 'Selected range'} · ${this.formatAxisTimestamp(history.from)} — ${this.formatAxisTimestamp(history.to)}`;
    }

    public axisLabels(): [string, string, string] {
        const history = this.history();
        if (!history) {
            return ['', '', ''];
        }

        const from = new Date(history.from).getTime();
        const to = new Date(history.to).getTime();
        const midpoint = new Date(from + ((to - from) / 2));

        return [
            this.formatAxisTimestamp(history.from),
            this.formatAxisTimestamp(midpoint.toISOString()),
            this.formatAxisTimestamp(history.to)
        ];
    }

    public ngOnDestroy(): void {
        this._destroy.next();
        this._destroy.complete();
    }

    private chartCoordinates(metric: Metric): Array<{ x: number; y: number }> {
        const samples = this.samples();

        return samples.map((sample) => ({
            x: this.chartPointX(sample),
            y: this.chartPointY(sample, metric)
        }));
    }
}
