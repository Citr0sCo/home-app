import { Component, Input, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import { QBitTorrentService } from '../../../../services/qbittorrent-service/qbittorrent.service';
import { IQBitTorrentStats } from '../../../../services/qbittorrent-service/types/qbittorrent-stats.type';

@Component({
    selector: 'qbittorrent-details',
    templateUrl: './qbittorrent-details.component.html',
    styleUrls: ['./qbittorrent-details.component.scss'],
    standalone: false
})
export class QBitTorrentDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    public stats: WritableSignal<IQBitTorrentStats | null> = signal<IQBitTorrentStats | null>(null);
    public formattedUploadRate: WritableSignal<string> = signal<string>('0 B/s');
    public formattedDownloadRate: WritableSignal<string> = signal<string>('0 B/s');
    public isLoading: WritableSignal<boolean> = signal<boolean>(true);

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _qBitTorrentService: QBitTorrentService;

    constructor(qBitTorrentService: QBitTorrentService) {
        this._qBitTorrentService = qBitTorrentService;
    }

    public ngOnInit(): void {
        this._qBitTorrentService.getStats(this.item?.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe({
                next: (stats: IQBitTorrentStats) => {
                    this.updateStats(stats);
                    this.isLoading.set(false);
                },
                error: () => this.isLoading.set(false)
            });

        this._qBitTorrentService.activities
            .asObservable()
            .pipe(takeUntil(this._destroy))
            .subscribe((activities: Array<IQBitTorrentStats>) => {
                const stats = activities.find((activity) => activity.identifier === this.item?.identifier) ?? null;
                if (stats) {
                    this.updateStats(stats);
                }
                this.isLoading.set(false);
            });

        this._qBitTorrentService.ngOnInit();
    }

    public ngOnDestroy(): void {
        this._qBitTorrentService.ngOnDestroy();
        this._destroy.next();
        this._destroy.complete();
    }

    private updateStats(stats: IQBitTorrentStats): void {
        this.stats.set(stats);
        this.formattedUploadRate.set(this.formatBytesPerSecond(stats.uploadRate));
        this.formattedDownloadRate.set(this.formatBytesPerSecond(stats.downloadRate));
    }

    private formatBytesPerSecond(bytesPerSecond: number): string {
        if (bytesPerSecond < 1024) {
            return `${bytesPerSecond} B/s`;
        }

        const units = ['kB/s', 'MB/s', 'GB/s'];
        const unitIndex = Math.min(Math.floor(Math.log(bytesPerSecond) / Math.log(1024)), units.length) - 1;
        const value = bytesPerSecond / Math.pow(1024, unitIndex + 1);

        return `${value.toFixed(value >= 10 ? 0 : 1)} ${units[unitIndex]}`;
    }
}
