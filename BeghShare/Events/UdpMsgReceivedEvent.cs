using System.Net;
using System.Net.Sockets;

namespace BeghShare.Events
{
    public record UdpMsgReceivedEvent
    {
        public byte[] Data { get; init; }
        public IPEndPoint RemoteEndPoint { get; init; }

        public UdpMsgReceivedEvent(UdpReceiveResult result)
        {
            Data = result.Buffer;
            RemoteEndPoint = result.RemoteEndPoint;
        }
    }
}
