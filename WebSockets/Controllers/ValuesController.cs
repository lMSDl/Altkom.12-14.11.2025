using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

namespace WebSockets.Controllers
{
    public class ValuesController : ControllerBase
    {
        //[HttpGet("/ws")] //Http 1.1
        [Route("/ws")] //Http 2.0
        public async Task Get()
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                HttpContext.Response.StatusCode = 400; // Bad Request
                return;
            }

            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            var helloMessage = "Hello from WebSocket server!";

            {
                var helloBuffer = Encoding.UTF8.GetBytes(helloMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(helloBuffer, 0, 5), WebSocketMessageType.Text, false, CancellationToken.None);
                await webSocket.SendAsync(new ArraySegment<byte>(helloBuffer, 5, helloBuffer.Length - 5), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            _ = Task.Run(async () =>
            {
                for (int i = 0; i < 3; i++)
                {
                    if (!webSocket.CloseStatus.HasValue && webSocket.State == WebSocketState.Open)
                    {
                        await Task.Delay(5000);
                        var ping = Encoding.UTF8.GetBytes("ping");
                        //wiadomość serwer => klient
                        await webSocket.SendAsync(new ArraySegment<byte>(ping), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        break;
                    }
                }
                //zamknięcie połączenia
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            });

            var buffer = new byte[10];
            WebSocketReceiveResult receiveResult;
            StringBuilder fullMessage = new StringBuilder();
            do
            {
                //wiadomość klient => serwer
                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (receiveResult.MessageType == WebSocketMessageType.Text)
                {
                    //EndOfMessage - czy to jest ostatni fragment wiadomości
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, receiveResult.Count), WebSocketMessageType.Text, receiveResult.EndOfMessage, CancellationToken.None);

                    var @string = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    fullMessage.Append(@string);


                    Debug.WriteLine(@string);

                    if (receiveResult.EndOfMessage)
                    {
                        Console.WriteLine(fullMessage);
                        fullMessage.Clear();
                    }
                }
            } while (!receiveResult.CloseStatus.HasValue);
        }
    }
}
