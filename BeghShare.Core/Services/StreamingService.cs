using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Attributes;
using BeghShare.Core.Events.MessageEvents;
using BeghShare.Core.Events.NetworkEvents;
using System.Net;

namespace BeghShare.Core.Services
{
    public class StreamingService : ISingleton, IAutoStart
    {
        private const string SendPeerControlRequestMsg = "BeghSendPeerControlRequestEvent:";
        private const string SendPeerControlAcceptMsg = "BeghSendPeerControlAcceptEvent:";

        [EventHandler]
        public async void OnSendPeerControlRequestEvent(SendPeerControlRequestEvent e)
        {
            SendEvent(new TcpMsgSendEvent
            {
                Header = SendPeerControlRequestMsg,
                Data = string.Empty,
                Ip = e.PeerInfo.IP
            });
        }

        [MsgEventHandler(SendPeerControlRequestMsg)]
        public async void OnSendPeerControlRequest(string _, IPAddress Ip)
        {
            var senderPeer = GetService<DiscoveryService>().GetPeerInfoByIpAddress(Ip);
            var senderName = senderPeer != null ? senderPeer.Name : "Unknown";
            if (senderPeer == null)
                GetService<DiscoveryService>().Discover(Ip);

            var ResponseReceived = GetService<IYesNoQustionService>().Handle(
                "Control Confirmation",
                $"Can The Peer {senderName}:{Ip} Controle You ?");

            //TODO: here i need to get more information about the target !! for exemple his screen resolution and fps !!
            if (ResponseReceived)
            {
                SendEvent(new TcpMsgSendEvent
                {
                    Header = SendPeerControlAcceptMsg,
                    Data = string.Empty,
                    Ip = Ip
                });
                if (senderPeer == null)
                    senderPeer = GetService<DiscoveryService>().GetPeerInfoByIpAddress(Ip)!;
                SendEvent(new PeerControlMeEvent() { PeerInfo = senderPeer });
            }
        }

        [MsgEventHandler(SendPeerControlAcceptMsg)]
        public async void OnSendPeerControlAcceptMsg(string _, IPAddress Ip)
        {
            var receivedPeer = GetService<DiscoveryService>().GetPeerInfoByIpAddress(Ip)!;
            SendEvent(new PeerStartControlingEvent() { PeerInfo = receivedPeer });
        }
    }
}
