using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
using BeghShare.Models;
using System.Net;
using System.Text;

namespace BeghShare.Services
{
    public class DiscoveryService : ISingleton
    {
        private const string BROADCAST_MSG = "BeghShareBroadcast:";
        private const string RESPONSE_MSG = "BeghShareResponse:";

        public List<PeerInfo> Peers = new();

        [EventHandler]
        public void OnUdpMsgReceived(UdpMsgReceivedEvent e)
        {
            string msg = Encoding.UTF8.GetString(e.Data);
            if (msg.StartsWith(BROADCAST_MSG))
            {
                string response = RESPONSE_MSG + Environment.MachineName;
                byte[] respBytes = Encoding.UTF8.GetBytes(response);
                Core.SendEvent(new UdpMsgSendEvent
                {
                    Data = respBytes,
                    RemoteEndPoint = e.RemoteEndPoint
                });
            }
            else if (msg.StartsWith(RESPONSE_MSG))
            {
                string name = msg.Substring(RESPONSE_MSG.Length);
                var peer = new PeerInfo { Name = name, IP = e.RemoteEndPoint.Address, IsOnline = true, IPEndPoint = e.RemoteEndPoint };
                if (!Peers.Exists(p => p.IP.Equals(peer.IP)))
                {
                    Peers.Add(peer);
                    Core.SendEvent(new PeerFoundEvent(peer));
                    Core.SendEvent(new OnlineComputerCountChangedEvent(Peers.Count()));
                }
            }
        }

        public void Discover()
        {
            byte[] data = Encoding.UTF8.GetBytes(BROADCAST_MSG);
            Core.SendEvent(new UdpMsgSendEvent
            {
                Data = data,
                RemoteEndPoint = new IPEndPoint(IPAddress.Broadcast, UdpTransportService.APPPORT_UDP)
            });
        }
    }
}
