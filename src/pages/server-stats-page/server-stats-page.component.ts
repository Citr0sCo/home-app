import { Component, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { catchError, EMPTY, map, merge, Subject, switchMap, takeUntil, timer } from 'rxjs';
import { StatService } from '../../services/stats-service/stat.service';
import { IStatResponse } from '../../services/stats-service/types/stat.response';
import { IStatHistoryResponse, IStatHistorySample } from '../../services/stats-service/types/stat-history.response';
import { HealthCheckHistoryRepository } from '../../services/health-check-service/health-check-history.repository';
import {
    IHealthCheckHistoryResponse,
    IHealthCheckHistorySample,
    IHealthCheckLinkHistory
} from '../../services/health-check-service/types/health-check-history.response';

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
    public healthHistory: WritableSignal<IHealthCheckHistoryResponse | null> = signal<IHealthCheckHistoryResponse | null>(null);
    public healthLoading: WritableSignal<boolean> = signal<boolean>(true);
    public healthError: WritableSignal<boolean> = signal<boolean>(false);

    private readonly _statService: StatService;
    private readonly _healthCheckHistoryRepository: HealthCheckHistoryRepository;
    private readonly _destroy: Subject<void> = new Subject();
    private readonly _rangeChanges: Subject<number> = new Subject();

    constructor(statService: StatService, healthCheckHistoryRepository: HealthCheckHistoryRepository) {
        this._statService = statService;
        this._healthCheckHistoryRepository = healthCheckHistoryRepository;
    }

    public ngOnInit(): void {
        this._statService.stats
            .pipe(takeUntil(this._destroy))
            .subscribe((stats) => this.appendLiveSample(stats));

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

        timer(0, 15000)
            .pipe(
                switchMap(() => this._healthCheckHistoryRepository.getHistory(7).pipe(
                    catchError(() => {
                        this.healthError.set(true);
                        this.healthLoading.set(false);
                        return EMPTY;
                    })
                )),
                takeUntil(this._destroy)
            )
            .subscribe({
                next: (healthHistory) => {
                    this.healthHistory.set(healthHistory);
                    this.healthError.set(healthHistory.hasError === true);
                    this.healthLoading.set(false);
                },
                error: () => {
                    this.healthError.set(true);
                    this.healthLoading.set(false);
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
        return point ? Math.max(75, Math.min(621, this.chartPointX(point.sample))) : 0;
    }

    public hoveredPointY(): number {
        const point = this.hoveredPoint();
        return point ? Math.max(56, this.chartPointY(point.sample, point.metric) - 8) : 0;
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

    public healthLinks(): Array<IHealthCheckLinkHistory> {
        return this.healthHistory()?.links ?? [];
    }

    public healthSamples(link: IHealthCheckLinkHistory): Array<IHealthCheckHistorySample> {
        return link.samples;
    }

    public healthLatest(link: IHealthCheckLinkHistory): IHealthCheckHistorySample | null {
        const samples = this.healthSamples(link);
        return samples.length === 0 ? null : samples[samples.length - 1];
    }

    public healthStatus(link: IHealthCheckLinkHistory): 'up' | 'warning' | 'down' | 'unknown' {
        const sample = this.healthLatest(link);
        if (!sample) {
            return 'unknown';
        }

        if (sample.statusCode >= 200 && sample.statusCode < 400) {
            return 'up';
        }
        if (sample.statusCode >= 400 && sample.statusCode < 500) {
            return 'warning';
        }
        return 'down';
    }

    public healthStatusLabel(link: IHealthCheckLinkHistory): string {
        const status = this.healthStatus(link);
        return status === 'unknown' ? 'No samples yet' : status;
    }

    public healthResponseTime(link: IHealthCheckLinkHistory): string {
        const sample = this.healthLatest(link);
        return sample ? this.formatDuration(sample.durationInMilliseconds) : '—';
    }

    public healthSparkline(link: IHealthCheckLinkHistory): string {
        return this.healthSamples(link)
            .map((sample, index, samples) => `${this.healthPointX(index, samples.length)},${this.healthPointY(sample, link)}`)
            .join(' ');
    }

    public healthPointX(index: number, sampleCount: number): number {
        return sampleCount <= 1 ? 120 : 4 + (index / (sampleCount - 1)) * 232;
    }

    public healthPointY(sample: IHealthCheckHistorySample, link: IHealthCheckLinkHistory): number {
        const maxDuration = Math.max(1000, ...this.healthSamples(link).map((item) => item.durationInMilliseconds));
        return 48 - Math.min(sample.durationInMilliseconds / maxDuration, 1) * 40;
    }

    public healthSparklineLabel(link: IHealthCheckLinkHistory): string {
        return `${link.name} response times over the last seven days`;
    }

    public formatDuration(duration: number): string {
        return duration >= 1000
            ? `${Math.round(duration / 10) / 100} s`
            : `${duration} ms`;
    }

    public ngOnDestroy(): void {
        this._destroy.next();
        this._destroy.complete();
    }

    private appendLiveSample(stats: IStatResponse | null): void {
        if (!stats || stats.stats.length === 0 || !this.history()) {
            return;
        }

        const memoryUsed = stats.stats.reduce((total, stat) => total + (stat.memoryUsage?.used ?? 0), 0);
        const memoryTotal = Math.max(...stats.stats.map((stat) => stat.memoryUsage?.total ?? 0));
        const diskUsed = Math.max(...stats.stats.map((stat) => stat.diskUsage?.used ?? 0));
        const diskTotal = Math.max(...stats.stats.map((stat) => stat.diskUsage?.total ?? 0));
        const recordedAt = new Date();
        const liveSample: IStatHistorySample = {
            recordedAt: recordedAt.toISOString(),
            cpuPercentage: Math.min(100, stats.stats.reduce((total, stat) => total + (stat.cpuUsage?.percentage ?? 0), 0)),
            memoryPercentage: memoryTotal > 0 ? (memoryUsed / memoryTotal) * 100 : 0,
            memoryUsed,
            memoryTotal,
            diskPercentage: diskTotal > 0 ? (diskUsed / diskTotal) * 100 : 0,
            diskUsed,
            diskTotal
        };
        const to = recordedAt.getTime();
        const from = to - this.selectedRangeHours() * 60 * 60 * 1000;
        const history = this.history()!;
        const samples = [...history.samples, liveSample]
            .filter((sample) => {
                const timestamp = new Date(sample.recordedAt).getTime();
                return timestamp >= from && timestamp <= to;
            })
            .sort((left, right) => new Date(left.recordedAt).getTime() - new Date(right.recordedAt).getTime());

        this.history.set({ ...history, from: new Date(from).toISOString(), to: recordedAt.toISOString(), samples });
    }
    private chartCoordinates(metric: Metric): Array<{ x: number; y: number }> {
        const samples = this.samples();

        return samples.map((sample) => ({
            x: this.chartPointX(sample),
            y: this.chartPointY(sample, metric)
        }));
    }
}
