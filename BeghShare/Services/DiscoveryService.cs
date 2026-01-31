using BeghCore;
using BeghShare.Attributes;
using BeghShare.Events;
using BeghShare.Models;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace BeghShare.Services
{
    public class DiscoveryService : ISingleton
    {
        private const string BROADCAST_MSG = "BeghShareBroadcast:";
        private const string RESPONSE_MSG = "BeghShareResponse:";

        public List<PeerInfo> Peers = new();

        [MsgEventHandler(BROADCAST_MSG)]
        public void OnBroadCastSentMsg(string Data, IPEndPoint e)
        {
            if (IsLocalAddress(e.Address)) return;
            string response = RESPONSE_MSG + Environment.MachineName;
            Core.SendEvent(new UdpMsgSendEvent
            {
                Data = response,
                RemoteEndPoint = e
            });

            if (!Peers.Any(p => p.IPEndPoint.Address.ToString() == e.Address.ToString()))
                Discover(e.Address.ToString());
        }

        [MsgEventHandler(RESPONSE_MSG)]
        public void OnBroadCastResponseMsg(string Data, IPEndPoint e)
        {
            if (IsLocalAddress(e.Address)) return;
            var peer = new PeerInfo { Name = Data, IP = e.Address, IsOnline = true, IPEndPoint = e };
            if (!Peers.Exists(p => p.IP.Equals(peer.IP)))
            {
                Peers.Add(peer);
                Core.SendEvent(new PeerFoundEvent(peer));
                Core.SendEvent(new OnlineComputerCountChangedEvent(Peers.Count()));
            }
        }

        //[EventHandler]
        //public void OnUdpMsgReceived(UdpMsgReceivedEvent e)
        //{
        //    if (IsLocalAddress(e.RemoteEndPoint.Address))
        //        return;

        //    if (e.Data.StartsWith(BROADCAST_MSG))
        //    {
        //        string response = RESPONSE_MSG + Environment.MachineName;
        //        Core.SendEvent(new UdpMsgSendEvent
        //        {
        //            Data = response,
        //            RemoteEndPoint = e.RemoteEndPoint
        //        });

        //        if (!Peers.Any(p => p.IPEndPoint.Address.ToString() == e.RemoteEndPoint.Address.ToString()))
        //            Discover(e.RemoteEndPoint.Address.ToString());
        //    }
        //    else if (e.Data.StartsWith(RESPONSE_MSG))
        //    {
        //        string name = e.Data.Substring(RESPONSE_MSG.Length);
        //        var peer = new PeerInfo { Name = name, IP = e.RemoteEndPoint.Address, IsOnline = true, IPEndPoint = e.RemoteEndPoint };
        //        if (!Peers.Exists(p => p.IP.Equals(peer.IP)))
        //        {
        //            Peers.Add(peer);
        //            Core.SendEvent(new PeerFoundEvent(peer));
        //            Core.SendEvent(new OnlineComputerCountChangedEvent(Peers.Count()));
        //        }
        //    }
        //}

        public void Discover()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up
                             && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback
                             && ni.SupportsMulticast);

            foreach (var ni in interfaces)
            {
                var props = ni.GetIPProperties();
                foreach (var unicast in props.UnicastAddresses.Where(u => u.Address.AddressFamily == AddressFamily.InterNetwork))
                {
                    var broadcast = unicast.IPv4Mask != null
                        ? GetBroadcastAddress(unicast.Address, unicast.IPv4Mask)
                        : unicast.Address;
                    Core.SendEvent(new UdpMsgSendEvent
                    {
                        Data = BROADCAST_MSG,
                        RemoteEndPoint = new IPEndPoint(broadcast, UdpTransportService.APPPORT_UDP)
                    });
                }
            }
        }

        public bool Discover(string ipString)
        {
            if (!IPAddress.TryParse(ipString, out var ip))
                return false;
            Core.SendEvent(new UdpMsgSendEvent
            {
                Data = BROADCAST_MSG,
                RemoteEndPoint = new IPEndPoint(ip, UdpTransportService.APPPORT_UDP)
            });
            return true;
        }
        private static IPAddress GetBroadcastAddress(IPAddress address, IPAddress mask)
        {
            var addrBytes = address.GetAddressBytes();
            var maskBytes = mask.GetAddressBytes();
            var broadcastBytes = new byte[4];
            for (int i = 0; i < 4; i++)
                broadcastBytes[i] = (byte)(addrBytes[i] | ~maskBytes[i]);
            return new IPAddress(broadcastBytes);
        }

        private static bool IsLocalAddress(IPAddress address)
        {
            if (IPAddress.IsLoopback(address))
                return true;

            var localAddresses = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up)
                .SelectMany(ni => ni.GetIPProperties().UnicastAddresses)
                .Select(u => u.Address)
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork);

            return localAddresses.Any(addr => addr.Equals(address));
        }
    }
}
