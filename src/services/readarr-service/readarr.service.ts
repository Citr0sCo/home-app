import { Injectable } from '@angular/core';
import { Observable, of, Subject, tap } from 'rxjs';
import { ReadarrRepository } from './readarr.repository';
import { IReadarrActivity } from './types/readarr-activity.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { ReadarrMapper } from './readarr.mapper';

@Injectable()
export class ReadarrService {

    public activity: Subject<IReadarrActivity> = new Subject<IReadarrActivity>();

    private _activity: IReadarrActivity | null = null;

    private _repository: ReadarrRepository;
    private _webSocketService: WebSocketService;

    constructor(repository: ReadarrRepository) {
        this._repository = repository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.ReadarrActivity, (payload: any) => {
            this.handleNewActivity(payload);
        });
    }

    public getActivity(): Observable<IReadarrActivity> {
        if (this._activity !== null) {
            return of(this._activity);
        }

        return this._repository.getActivity()
            .pipe(tap((activity: IReadarrActivity) => {
                this._activity = activity;
                this.activity.next(this._activity);
            }));
    }

    public handleNewActivity(payload: any): void {
        this._activity = ReadarrMapper.mapWebsocketActivities(payload);
        this.activity.next(this._activity);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.ReadarrActivity);
    }

}