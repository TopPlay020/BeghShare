using System.Net;

namespace BeghShare.Events
{
    public record TcpMsgReceivedEvent
    {
        public required string Data { get; init; }
        public required IPAddress Ip { get; init; }
    }
}
