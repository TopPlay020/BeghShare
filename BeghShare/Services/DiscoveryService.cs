using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
using BeghShare.Models;
using System.Net;

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
            if (e.Data.StartsWith(BROADCAST_MSG))
            {
                string response = RESPONSE_MSG + Environment.MachineName;
                Core.SendEvent(new UdpMsgSendEvent
                {
                    Data = response,
                    RemoteEndPoint = e.RemoteEndPoint
                });
            }
            else if (e.Data.StartsWith(RESPONSE_MSG))
            {
                string name = e.Data.Substring(RESPONSE_MSG.Length);
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
            Core.SendEvent(new UdpMsgSendEvent
            {
                Data = BROADCAST_MSG,
                RemoteEndPoint = new IPEndPoint(IPAddress.Broadcast, UdpTransportService.APPPORT_UDP)
            });
        }
    }
}
