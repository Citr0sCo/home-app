using System.Net.WebSockets;

namespace HomeBoxLanding.Api.Features.WebSockets;

public class InternalWebSocket
{
    public DateTime LastSeen { get; set; }

    private readonly WebSocket _socket;
    private readonly object _sendLock = new();

    public InternalWebSocket(WebSocket socket)
    {
        _socket = socket;
    }

    public bool HasDisconnected()
    {
        return DateTime.UtcNow - LastSeen > TimeSpan.FromMinutes(2);
    }

    public void SendAsync(ArraySegment<byte> data, WebSocketMessageType messageType, WebSocketMessageFlags flags, CancellationToken cancellationToken)
    {
        lock (_sendLock)
        {
            if (_socket.State != WebSocketState.Open)
                return;

            _socket.SendAsync(data, messageType, flags, cancellationToken).GetAwaiter().GetResult();
            LastSeen = DateTime.UtcNow;
        }
    }

    public void Close(string reason, CancellationToken cancellationToken)
    {
        lock (_sendLock)
        {
            if (_socket.State is WebSocketState.Closed or WebSocketState.Aborted)
                return;

            _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, reason, cancellationToken)
                .GetAwaiter()
                .GetResult();
        }
    }
}