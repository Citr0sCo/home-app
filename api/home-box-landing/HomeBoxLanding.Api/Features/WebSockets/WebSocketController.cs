using HomeBoxLanding.Api.Features.WebSockets.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.WebSockets
{
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketManager _webSocketManager;

        public WebSocketController()
        {
            _webSocketManager = WebSocketManager.Instance();
        }

        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var buffer = new byte[1024 * 4];
                    var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var sessionId = Guid.NewGuid();

                    while (receiveResult.CloseStatus.HasValue == false)
                    {
                        _webSocketManager.Receive(sessionId, buffer, webSocket);
                        receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    }

                    await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}