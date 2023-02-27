import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable()
export class WebSocketService {

    private _sessionId: string = '';
    private _webSocket: WebSocket | null = null;

    constructor() {
        this._webSocket = new WebSocket(`wss://${environment.webSocketUrl}/ws`);

        this._webSocket.onopen = this.handleOpen;
        this._webSocket.onmessage = this.handleMessage;
        this._webSocket.onclose = this.handleClose;
    }

    public send(payload: any): void {
        this._webSocket?.send(JSON.stringify(payload));
    }

    public handleOpen(): void {
        console.log('WebSocket connection is open...');
    }

    public handleMessage(e: any): void {
        console.log('WebSocket message received.');

        const response = JSON.parse(e.data);
        if (response.Key === 'Handshake') {
            this._sessionId = response.Data;
        }
    }

    public handleClose(): void {
        console.log('WebSocket connection is closed...');
    }
}