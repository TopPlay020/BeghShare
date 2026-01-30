using System.Net;

namespace BeghShare.Events
{
    public record UdpMsgReceivedEvent
    {
        public required string Data { get; init; }
        public required IPEndPoint RemoteEndPoint { get; init; }
    }
}
