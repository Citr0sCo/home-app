import { Injectable } from '@angular/core';
import { Observable, of, Subject, tap } from 'rxjs';
import { StatRepository } from './stat.repository';
import { IStatResponse } from './types/stat.response';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { StatMapper } from './stat.mapper';

@Injectable()
export class StatService {

    public stats: Subject<IStatResponse | null> = new Subject<IStatResponse | null>();

    private _statsCache: IStatResponse | null = null;

    private _statRepository: StatRepository;
    private _webSocketService: WebSocketService;

    constructor(deployRepository: StatRepository) {
        this._statRepository = deployRepository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.ServerStats, (payload: any) => {
            this.handleNewStats(payload);
        });
    }

    public getAll(): Observable<IStatResponse> {
        if (this._statsCache !== null) {
            return of(this._statsCache);
        }

        return this._statRepository.getAll()
            .pipe(tap((deploys: IStatResponse) => {
                this._statsCache = deploys;
            }));
    }

    public handleNewStats(payload: any): void {
        this._statsCache = StatMapper.map(payload);
        this.stats.next(this._statsCache);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.ServerStats);
    }
}