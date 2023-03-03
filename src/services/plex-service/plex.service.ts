import { Injectable } from '@angular/core';
import { Observable, of, Subject, tap } from 'rxjs';
import { PlexRepository } from './plex.repository';
import { IPlexSession } from './types/plex-session.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { PlexMapper } from './plex.mapper';

@Injectable()
export class PlexService {

    public sessions: Subject<Array<IPlexSession>> = new Subject<Array<IPlexSession>>();

    private _sessions: Array<IPlexSession> = new Array<IPlexSession>();

    private _plexRepository: PlexRepository;
    private _webSocketService: WebSocketService;

    constructor(deployRepository: PlexRepository) {
        this._plexRepository = deployRepository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.PlexActivity, (payload: any) => {
            this.handleNewPlexActivity(payload);
        });
    }

    public getActivity(): Observable<Array<IPlexSession>> {
        if (this._sessions.length > 0) {
            return of(this._sessions);
        }

        return this._plexRepository.getActivity()
            .pipe(tap((plexSessions: Array<IPlexSession>) => {
                this._sessions = plexSessions;
            }));
    }

    public handleNewPlexActivity(payload: any): void {
        this._sessions = PlexMapper.mapActivity(payload);
        this.sessions.next(this._sessions);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.PlexActivity);
    }

}