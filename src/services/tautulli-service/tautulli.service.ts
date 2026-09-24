import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { TautulliMapper } from './tautulli.mapper';
import { TautulliRepository } from './tautulli.repository';
import { ITautulliStats } from './types/tautulli-stats.type';

@Injectable()
export class TautulliService {

    public activities: Subject<Array<ITautulliStats>> = new Subject<Array<ITautulliStats>>();

    private readonly _repository: TautulliRepository;
    private readonly _webSocketService: WebSocketService;

    constructor(
        repository: TautulliRepository,
        webSocketService: WebSocketService = WebSocketService.instance()) {
        this._repository = repository;
        this._webSocketService = webSocketService;
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.TautulliStats, (payload: any) => {
            this.handleNewActivity(payload);
        });
    }

    public getStats(identifier: string): Observable<ITautulliStats> {
        return this._repository.getStats(identifier);
    }

    public handleNewActivity(payload: any): void {
        this.activities.next(TautulliMapper.mapActivities(payload));
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.TautulliStats);
    }
}
