import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { QBitTorrentMapper } from './qbittorrent.mapper';
import { QBitTorrentRepository } from './qbittorrent.repository';
import { IQBitTorrentStats } from './types/qbittorrent-stats.type';

@Injectable()
export class QBitTorrentService {

    public activities: Subject<Array<IQBitTorrentStats>> = new Subject<Array<IQBitTorrentStats>>();

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
        return this._repository.getStats(identifier);
    }

    public handleNewActivity(payload: any): void {
        this.activities.next(QBitTorrentMapper.mapActivities(payload));
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.QBitTorrentStats);
    }
}
