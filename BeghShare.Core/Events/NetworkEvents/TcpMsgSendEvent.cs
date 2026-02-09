using System.Net;

namespace BeghShare.Core.Events.NetworkEvents
{
    public record TcpMsgSendEvent
    {
        public string Header { get; set; }
        public string Data { get; set; }
        public bool IsEncrypted { get; set; } = false;
        public IPAddress Ip { get; set; }
    }
}
