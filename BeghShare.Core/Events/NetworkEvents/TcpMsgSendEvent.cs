using System.Net;

namespace BeghShare.Core.Events.NetworkEvents
{
    public record TcpMsgSendEvent
    {
        public string Header { get; set; }
        public string Data { get; set; }
        public IPAddress Ip { get; set; }
    }
}
