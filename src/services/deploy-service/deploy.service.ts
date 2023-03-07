import { Injectable } from '@angular/core';
import { IDeploy } from './types/deploy.type';
import { Observable, of, Subject, tap } from 'rxjs';
import { DeployRepository } from './deploy.repository';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';
import { DeployMapper } from './deploy.mapper';

@Injectable()
export class DeployService {

    public deploys: Subject<Array<IDeploy>> = new Subject<Array<IDeploy>>();

    private _deployCache: Array<IDeploy> = new Array<IDeploy>();
    private _deployRepository: DeployRepository;
    private _webSocketService: WebSocketService;

    constructor(deployRepository: DeployRepository) {
        this._deployRepository = deployRepository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.DeployStarted, (payload: any) => {
            this.handleDeployStarted(payload);
        });
        this._webSocketService.subscribe(WebSocketKey.DeployUpdated, (payload: any) => {
            this.handleDeployUpdated(payload);
        });
    }

    public getAll(): Observable<Array<IDeploy>> {
        if (this._deployCache.length > 0) {
            return of(this._deployCache);
        }

        return this._deployRepository.getAll()
            .pipe(tap((deploys: Array<IDeploy>) => {
                this._deployCache = deploys;
            }));
    }

    public handleDeployStarted(payload: any): void {
        this._deployCache.push(DeployMapper.mapSingle(payload));
        this.deploys.next(this._deployCache);
    }

    public handleDeployUpdated(payload: any): void {
        this._deployCache = this._deployCache.map((deploy) => {

            if (deploy.identifier === payload.DeployIdentifier) {
                deploy.finishedAt = new Date(payload.FinishedAt);
            }

            return deploy;
        });
        this.deploys.next(this._deployCache);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.DeployStarted);
        this._webSocketService.unsubscribe(WebSocketKey.DeployUpdated);
    }
}