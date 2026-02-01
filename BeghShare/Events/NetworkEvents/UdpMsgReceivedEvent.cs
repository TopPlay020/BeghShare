using System.Net;

namespace BeghShare.Events.NetworkEvents
{
    public record UdpMsgReceivedEvent
    {
        public required string Data { get; init; }
        public required IPAddress Ip { get; init; }
    }
}
