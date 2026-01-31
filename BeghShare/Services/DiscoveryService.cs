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

        private List<PeerInfo> Peers = new();

        [MsgEventHandler(BROADCAST_MSG)]
        public void OnBroadCastSentMsg(string Data, IPAddress Ip)
        {
            if (IsLocalAddress(Ip)) return;
            string response = RESPONSE_MSG + Environment.MachineName;
            Core.SendEvent(new UdpMsgSendEvent
            {
                Data = response,
                Ip = Ip
            });

            if (GetPeerInfoByIpAddress(Ip) == null)
                Discover(Ip.ToString());
        }

        [MsgEventHandler(RESPONSE_MSG)]
        public void OnBroadCastResponseMsg(string Data, IPAddress Ip)
        {
            if (IsLocalAddress(Ip)) return;
            var peer = new PeerInfo { Name = Data, IP = Ip, IsOnline = true };
            if (!Peers.Exists(p => p.IP.Equals(peer.IP)))
            {
                Peers.Add(peer);
                Core.SendEvent(new PeerFoundEvent(peer));
                Core.SendEvent(new OnlineComputerCountChangedEvent(Peers.Count()));
            }
        }

        public PeerInfo? GetPeerInfoByIpAddress(IPAddress ipAddress)
        {
            return Peers.FirstOrDefault(p => p.IP.Equals(ipAddress));
        }

        public List<PeerInfo> GetPeerInfos() => Peers.ToList();

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
                        Ip = broadcast
                    });
                }
            }
        }

        public bool Discover(string ipString)
        {
            if (!IPAddress.TryParse(ipString, out var ip))
                return false;
            return Discover(ip);
        }

        public bool Discover(IPAddress ip)
        {
            Core.SendEvent(new UdpMsgSendEvent
            {
                Data = BROADCAST_MSG,
                Ip = ip
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
