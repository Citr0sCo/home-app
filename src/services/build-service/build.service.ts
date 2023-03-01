import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BuildRepository } from './build.repository';
import { IBuild } from './types/build.type';
import { WebSocketService } from '../websocket-service/web-socket.service';
import { WebSocketKey } from '../websocket-service/types/web-socket.key';

@Injectable()
export class BuildService implements OnInit, OnDestroy {

    private _statRepository: BuildRepository;
    private _webSocketService: WebSocketService;

    constructor(deployRepository: BuildRepository) {
        this._statRepository = deployRepository;
        this._webSocketService = WebSocketService.instance();
    }

    public ngOnInit(): void {
        this._webSocketService.subscribe(WebSocketKey.BuildStarted, this.handleBuildStarted);
        this._webSocketService.subscribe(WebSocketKey.BuildUpdated, this.handleBuildUpdated);
    }

    public getAll(): Observable<Array<IBuild>> {
        return this._statRepository.getAll();
    }

    public handleBuildStarted(payload: any): void {
        console.log('started: ', payload);
    }

    public handleBuildUpdated(payload: any): void {
        console.log('updated: ', payload);
    }

    public ngOnDestroy(): void {
        this._webSocketService.unsubscribe(WebSocketKey.BuildStarted);
        this._webSocketService.unsubscribe(WebSocketKey.BuildUpdated);
    }

}