import { Injectable } from '@angular/core';
import { Observable, of, Subject, tap } from 'rxjs';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { TautulliMapper } from './tautulli.mapper';
import { TautulliRepository } from './tautulli.repository';
import { ITautulliStats } from './types/tautulli-stats.type';

@Injectable()
export class TautulliService {

    public activities: Subject<Array<ITautulliStats>> = new Subject<Array<ITautulliStats>>();

    private _activities: Array<ITautulliStats> = [];
    private readonly _repository: TautulliRepository;
    private readonly _webSocketService: WebSocketService;

    constructor(repository: TautulliRepository) {
        this._repository = repository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.TautulliStats, (payload: any) => {
            this.handleNewActivity(payload);
        });
    }

    public getStats(identifier: string): Observable<ITautulliStats> {
        const cachedStats = this._activities.find((activity) => activity.identifier === identifier);

        if (cachedStats) {
            return of(cachedStats);
        }

        return this._repository.getStats(identifier)
            .pipe(tap((stats) => this.updateActivity(stats)));
    }

    public handleNewActivity(payload: any): void {
        this._activities = TautulliMapper.mapActivities(payload);
        this.activities.next(this._activities);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.TautulliStats);
    }

    private updateActivity(stats: ITautulliStats): void {
        this._activities = [
            ...this._activities.filter((activity) => activity.identifier !== stats.identifier),
            stats
        ];
    }
}
