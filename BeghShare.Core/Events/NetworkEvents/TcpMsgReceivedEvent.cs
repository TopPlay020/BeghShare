using System.Net;

namespace BeghShare.Core.Events.NetworkEvents
{
    public record TcpMsgReceivedEvent
    {
        public string Data { get; set; }
        public IPAddress Ip { get; set; }
    }
}
