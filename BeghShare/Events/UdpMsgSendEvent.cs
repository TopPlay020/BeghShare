using System.Net;

namespace BeghShare.Events
{
    public record UdpMsgSendEvent
    {
        public required byte[] Data { get; init; }
        public required IPEndPoint RemoteEndPoint { get; init; }
    }
}
