using System.Net;

namespace BeghShare.Events
{
    public record TcpMsgSendEvent
    {
        public required string Data { get; init; }
        public required IPAddress Ip { get; init; }
    }
}
