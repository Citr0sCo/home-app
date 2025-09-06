import { Injectable } from '@angular/core';
import { Observable, of, Subject, tap } from 'rxjs';
import { LidarrRepository } from './lidarr.repository';
import { ILidarrActivity } from './types/radarr-activity.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { LidarrMapper } from './lidarr.mapper';

@Injectable()
export class LidarrService {

    public activity: Subject<ILidarrActivity> = new Subject<ILidarrActivity>();

    private _activity: ILidarrActivity | null = null;

    private _repository: LidarrRepository;
    private _webSocketService: WebSocketService;

    constructor(repository: LidarrRepository) {
        this._repository = repository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.LidarrActivity, (payload: any) => {
            this.handleNewActivity(payload);
        });
    }

    public getActivity(): Observable<ILidarrActivity> {
        if (this._activity !== null) {
            return of(this._activity);
        }

        return this._repository.getActivity()
            .pipe(tap((activity: ILidarrActivity) => {
                this._activity = activity;
                this.activity.next(this._activity);
            }));
    }

    public handleNewActivity(payload: any): void {
        this._activity = LidarrMapper.mapWebsocketActivities(payload);
        this.activity.next(this._activity);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.LidarrActivity);
    }

}