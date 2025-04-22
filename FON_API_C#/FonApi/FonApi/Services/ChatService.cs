using Confluent.Kafka;
using System.Net.WebSockets;
using System.Text;

namespace FonApi.Services
{
    public class ChatService
    {
        private readonly List<WebSocket> _sockets = new();

        public async Task HandleWebSocketConnection(WebSocket socket)
        {
            _sockets.Add(socket);
            await SendMessage(System.Text.Json.JsonSerializer.Serialize(new { type = "[Kafka] Add message", message = "Conectado" }));
            var buffer = new byte[1024 * 2];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, default);
                    break;
                }

                foreach (var s in _sockets)
                {
                    await s.SendAsync(buffer[..result.Count], WebSocketMessageType.Text, true, default);
                }
            }
            _sockets.Remove(socket);
        }

        public async Task SendMessage(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);

            foreach (var s in _sockets)
            {
                await s.SendAsync(
                    new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
        }
    }
}
