import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { UptimeKumaRepository } from './uptime-kuma-repository';
import { IUptimeKumaActivity } from './types/uptime-kuma-activity.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { UptimeKumaMapper } from './uptime-kuma.mapper';

@Injectable()
export class UptimeKumaService {

    public activities: Subject<Array<IUptimeKumaActivity>> = new Subject<Array<IUptimeKumaActivity>>();

    private _repository: UptimeKumaRepository;
    private _webSocketService: WebSocketService;

    constructor(repository: UptimeKumaRepository) {
        this._repository = repository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.UptimeKumaActivity, (payload: any) => {
            this.handleNewActivity(payload);
        });
    }

    public getActivity(identifier: string): Observable<IUptimeKumaActivity> {
        return this._repository.getActivity(identifier);
    }

    public handleNewActivity(payload: any): void {
        this.activities.next(UptimeKumaMapper.mapActivities(payload));
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.UptimeKumaActivity);
    }

}