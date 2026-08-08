import { WebSocketService } from './web-socket.service';
import { WebSocketKey } from './types/web-socket.key';

class FakeWebSocket {
    public static INSTANCES: Array<FakeWebSocket> = [];
    public onopen: (() => void) | null = null;
    public onmessage: ((event: { data: string }) => void) | null = null;
    public onclose: (() => void) | null = null;
    public onerror: ((event: unknown) => void) | null = null;
    public sent: Array<string> = [];
    public readonly url: string;

    public constructor(url: string) {
        this.url = url;
        FakeWebSocket.INSTANCES.push(this);
    }

    public send(payload: string): void {
        this.sent.push(payload);
    }
}

describe('WebSocketService', () => {
    const originalWebSocket = globalThis.WebSocket;

    beforeEach(() => {
        localStorage.clear();
        FakeWebSocket.INSTANCES = [];
        (globalThis as { WebSocket: typeof WebSocket }).WebSocket = FakeWebSocket as unknown as typeof WebSocket;
        Object.defineProperty(WebSocketService, '_INSTANCE', { value: null, writable: true });
        vi.useFakeTimers();
    });

    afterEach(() => {
        (globalThis as { WebSocket: typeof WebSocket }).WebSocket = originalWebSocket;
        Object.defineProperty(WebSocketService, '_INSTANCE', { value: null, writable: true });
        vi.clearAllTimers();
        vi.useRealTimers();
    });

    it('queues messages until ready and sends them with the session', () => {
        const service = WebSocketService.instance();
        const socket = FakeWebSocket.INSTANCES[0];

        expect(socket.url).toBe('wss://localhost:7058/ws');
        service.send(WebSocketKey.ServerStats, { refresh: true });
        expect(socket.sent).toEqual([]);

        service.handleOpen();
        vi.advanceTimersByTime(1000);

        expect(socket.sent).toEqual([JSON.stringify({
            Key: WebSocketKey.ServerStats,
            Data: { refresh: true },
            SessionId: null
        })]);
    });

    it('stores handshakes and notifies subscribers of messages', () => {
        const service = WebSocketService.instance();
        const callback = vi.fn();
        service.subscribe(WebSocketKey.PlexActivity, callback);
        service.subscribe(WebSocketKey.PlexActivity, callback);

        service.handleMessage({ data: JSON.stringify({ Key: WebSocketKey.Handshake, Data: 'session-id' }) });
        expect(localStorage.getItem('sessionId')).toBe('session-id');

        service.handleMessage({ data: JSON.stringify({ Key: WebSocketKey.PlexActivity, Data: { title: 'Movie' } }) });
        expect(callback).toHaveBeenCalledTimes(2);
        expect(callback).toHaveBeenCalledWith({ title: 'Movie' });
    });

    it('sends unsubscriptions and publishes connection lifecycle state', () => {
        const service = WebSocketService.instance();
        const socket = FakeWebSocket.INSTANCES[0];
        const connectionStates: Array<boolean> = [];
        service.isConnected.subscribe((state) => connectionStates.push(state));

        service.handleOpen();
        service.unsubscribe({ Key: WebSocketKey.ServerStats });
        service.handleClose();
        service.handleError(new Error('failed'));

        expect(socket.sent).toEqual([JSON.stringify({ Key: WebSocketKey.ServerStats })]);
        expect(connectionStates).toEqual([true, false]);
        expect(localStorage.getItem('sessionId')).toBeNull();
    });
});
