import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { PiHoleRepository } from './pi-hole-repository';
import { IPiHoleActivity } from './types/pihole-activity.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { PiHoleMapper } from './pi-hole.mapper';

@Injectable()
export class PiHoleService {

    public activities: Subject<Array<IPiHoleActivity>> = new Subject<Array<IPiHoleActivity>>();

    private _repository: PiHoleRepository;
    private _webSocketService: WebSocketService;

    constructor(repository: PiHoleRepository) {
        this._repository = repository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.PiHoleActivity, (payload: any) => {
            this.handleNewActivity(payload);
        });
    }

    public getActivity(identifier: string): Observable<IPiHoleActivity> {
        return this._repository.getActivity(identifier);
    }

    public handleNewActivity(payload: any): void {
        this.activities.next(PiHoleMapper.mapActivities(payload));
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.PiHoleActivity);
    }

}