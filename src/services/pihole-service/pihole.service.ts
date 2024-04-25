import { Injectable } from '@angular/core';
import { Observable, of, Subject, tap } from 'rxjs';
import { PiholeRepository } from './pihole.repository';
import { IPiholeActivity } from './types/pihole-activity.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { PiholeMapper } from './pihole.mapper';

@Injectable()
export class PiholeService {

    public activities: Subject<Array<IPiholeActivity>> = new Subject<Array<IPiholeActivity>>();

    private _activities: Array<IPiholeActivity> = [];

    private _repository: PiholeRepository;
    private _webSocketService: WebSocketService;

    constructor(repository: PiholeRepository) {
        this._repository = repository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.PiholeActivity, (payload: any) => {
            this.handleNewActivity(payload);
        });
    }

    public getActivity(identifier: string): Observable<IPiholeActivity> {
        if (this._activities.length > 0) {
            return of(this._activities.find((x) => x.identifier === identifier)!);
        }

        return this._repository.getActivity(identifier)
            .pipe(tap((activity: IPiholeActivity) => {
                this._activities = this._activities.map((x) => {

                    if (x.identifier === activity.identifier) {
                        x.queriesToday = activity.queriesToday;
                        x.blockedToday = activity.blockedToday;
                        x.blockedPercentage = activity.blockedPercentage;
                    }

                    return x;
                });
            }));
    }

    public handleNewActivity(payload: any): void {
        this._activities = PiholeMapper.mapActivities(payload);
        this.activities.next(this._activities);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.PlexActivity);
    }

}