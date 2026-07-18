import { Injectable } from '@angular/core';
import { Observable, of, Subject, tap } from 'rxjs';
import { UptimeKumaRepository } from './uptime-kuma-repository';
import { IUptimeKumaActivity } from './types/uptime-kuma-activity.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { UptimeKumaMapper } from './uptime-kuma.mapper';

@Injectable()
export class UptimeKumaService {

    public activities: Subject<Array<IUptimeKumaActivity>> = new Subject<Array<IUptimeKumaActivity>>();

    private _activities: Array<IUptimeKumaActivity> = [];

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
        return this._repository.getActivity(identifier)
            .pipe(tap((activity: IUptimeKumaActivity) => {
                this._activities = this._activities.map((x) => {
                    x.metrics = activity.metrics.map((y) => {
                        return {
                            name: y.name,
                            isUp: y.isUp
                        };
                    });
                    return x;
                });
            }));
    }

    public handleNewActivity(payload: any): void {
        this._activities = UptimeKumaMapper.mapActivities(payload);
        this.activities.next(this._activities);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.PlexActivity);
    }

}