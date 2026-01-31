using System.Net;

namespace BeghShare.Events
{
    public record UdpMsgSendEvent
    {
        public required string Header { get; init; }
        public required string Data { get; init; }
        public required IPAddress Ip { get; init; }
    }
}
