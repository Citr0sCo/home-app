import { Injectable } from '@angular/core';
import { Observable, of, Subject, tap } from 'rxjs';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { QBitTorrentMapper } from './qbittorrent.mapper';
import { QBitTorrentRepository } from './qbittorrent.repository';
import { IQBitTorrentStats } from './types/qbittorrent-stats.type';

@Injectable()
export class QBitTorrentService {

    public activities: Subject<Array<IQBitTorrentStats>> = new Subject<Array<IQBitTorrentStats>>();

    private _activities: Array<IQBitTorrentStats> = [];
    private readonly _repository: QBitTorrentRepository;
    private readonly _webSocketService: WebSocketService;

    constructor(repository: QBitTorrentRepository) {
        this._repository = repository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.QBitTorrentStats, (payload: any) => {
            this.handleNewActivity(payload);
        });
    }

    public getStats(identifier: string): Observable<IQBitTorrentStats> {
        const cachedStats = this._activities.find((activity) => activity.identifier === identifier);

        if (cachedStats) {
            return of(cachedStats);
        }

        return this._repository.getStats(identifier)
            .pipe(tap((stats) => this.updateActivity(stats)));
    }

    public handleNewActivity(payload: any): void {
        this._activities = QBitTorrentMapper.mapActivities(payload);
        this.activities.next(this._activities);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.QBitTorrentStats);
    }

    private updateActivity(stats: IQBitTorrentStats): void {
        this._activities = [
            ...this._activities.filter((activity) => activity.identifier !== stats.identifier),
            stats
        ];
    }
}
