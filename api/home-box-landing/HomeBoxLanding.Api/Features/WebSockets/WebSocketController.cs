using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.WebSockets;

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
        try 
        {
            if (_webSocketManager.CancellationToken().IsCancellationRequested)
                return;
            
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var buffer = new byte[1024 * 4];
                    var message = new MemoryStream();
                    var sessionId = Guid.NewGuid();
                    WebSocketReceiveResult receiveResult;

                    do
                    {
                        receiveResult = await webSocket.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            _webSocketManager.CancellationToken());

                        if (receiveResult.MessageType == WebSocketMessageType.Close)
                            break;

                        message.Write(buffer, 0, receiveResult.Count);
                        if (receiveResult.EndOfMessage)
                        {
                            _webSocketManager.Receive(sessionId, message.ToArray(), webSocket);
                            message.SetLength(0);
                        }
                    }
                    while (receiveResult.CloseStatus.HasValue == false);

                    if (webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived)
                    {
                        await webSocket.CloseAsync(
                            receiveResult.CloseStatus ?? WebSocketCloseStatus.NormalClosure,
                            receiveResult.CloseStatusDescription,
                            _webSocketManager.CancellationToken());
                    }
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An unknown exception occured whilst receiving a socket request. Exception below:");
            Console.WriteLine(e.Message);
            Console.WriteLine(JsonConvert.SerializeObject(e));
        }
    }
}