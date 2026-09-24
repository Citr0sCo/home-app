import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { PlexRepository } from './plex.repository';
import { IPlexSession } from './types/plex-session.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { PlexMapper } from './plex.mapper';

@Injectable()
export class PlexService {

    public sessions: Subject<Array<IPlexSession>> = new Subject<Array<IPlexSession>>();

    private _repository: PlexRepository;
    private _webSocketService: WebSocketService;

    constructor(
        repository: PlexRepository,
        webSocketService: WebSocketService = WebSocketService.instance()) {
        this._repository = repository;
        this._webSocketService = webSocketService;
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.PlexActivity, (payload: any) => {
            this.handleNewActivity(payload);
        });
    }

    public getActivity(): Observable<Array<IPlexSession>> {
        return this._repository.getActivity();
    }

    public handleNewActivity(payload: any): void {
        this.sessions.next(PlexMapper.mapActivity(payload));
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.PlexActivity);
    }

}