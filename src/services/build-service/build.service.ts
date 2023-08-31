import { Injectable } from '@angular/core';
import { Observable, of, Subject, tap } from 'rxjs';
import { BuildRepository } from './build.repository';
import { IBuild } from './types/build.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { BuildMapper } from './build.mapper';

@Injectable()
export class BuildService {

    public builds: Subject<Array<IBuild>> = new Subject<Array<IBuild>>();

    private _buildCache: Array<IBuild> = new Array<IBuild>();
    private _buildRepository: BuildRepository;
    private _webSocketService: WebSocketService;

    constructor(deployRepository: BuildRepository) {
        this._buildRepository = deployRepository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.BuildStarted, (payload: any) => {
            this.handleBuildStarted(payload);
        });
        this._webSocketService.subscribe(WebSocketKey.BuildUpdated, (payload: any) => {
            this.handleBuildUpdated(payload);
        });
    }

    public getAll(): Observable<Array<IBuild>> {
        if (this._buildCache.length > 0) {
            return of(this._buildCache);
        }

        return this._buildRepository.getAll()
            .pipe(tap((builds: Array<IBuild>) => {
                this._buildCache = builds;
            }));
    }

    public updateAllDockerApps(): Observable<void> {
        return this._buildRepository.updateAllDockerApps()
            .pipe(tap(() => {
                //this._buildCache = builds;
            }));
    }

    public handleBuildStarted(payload: any): void {
        this._buildCache.push(BuildMapper.mapSingle(payload));
        this.builds.next(this._buildCache);
    }

    public handleBuildUpdated(payload: any): void {
        this._buildCache = this._buildCache.map((build) => {

            if (build.identifier === payload.Identifier) {
                build.conclusion = payload.Conclusion;
                build.status = payload.Status;
                build.finishedAt = new Date(payload.FinishedAt);
            }

            return build;
        });
        this.builds.next(this._buildCache);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.BuildStarted);
        this._webSocketService.unsubscribe(WebSocketKey.BuildUpdated);
    }

}