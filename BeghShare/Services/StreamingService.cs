using BeghCore;
using BeghCore.Attributes;
using BeghShare.Attributes;
using BeghShare.Events;
using System.Net;

namespace BeghShare.Services
{
    public class StreamingService : ISingleton, IAutoStart
    {
        private const string SendPeerControlRequestMsg = "BeghSendPeerControlRequestEvent:";
        private const string SendPeerControlAcceptMsg = "BeghSendPeerControlAcceptEvent:";

        [EventHandler]
        public async void OnSendPeerControlRequestEvent(SendPeerControlRequestEvent e)
        {
            Core.SendEvent(new TcpMsgSendEvent
            {
                Header = SendPeerControlRequestMsg,
                Data = string.Empty,
                Ip = e.PeerInfo.IP
            });
        }

        [MsgEventHandler(SendPeerControlRequestMsg)]
        public async void OnSendPeerControlRequest(string data, IPAddress Ip)
        {
            var senderPeer = Core.GetService<DiscoveryService>().GetPeerInfoByIpAddress(Ip);
            var senderName = senderPeer != null ? senderPeer.Name : "Unknown";
            if (senderPeer == null)
                Core.GetService<DiscoveryService>().Discover(Ip);
            var mainWindow = Core.GetService<MainWindow>();
            mainWindow.Invoke((MethodInvoker)(() =>
            {
                var result = MessageBox.Show(
                    mainWindow,
                    $"{senderName}:{Ip} Want To Control You",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Core.SendEvent(new TcpMsgSendEvent
                    {
                        Header = SendPeerControlAcceptMsg,
                        Data = string.Empty,
                        Ip = Ip
                    });
                    if (senderPeer == null)
                        senderPeer = Core.GetService<DiscoveryService>().GetPeerInfoByIpAddress(Ip)!;
                    Core.SendEvent(new PeerControlMeEvent() { PeerInfo = senderPeer });
                }
            }));
        }

        [MsgEventHandler(SendPeerControlAcceptMsg)]
        public async void OnSendPeerControlAcceptMsg(string data, IPAddress Ip)
        {
            var receivedPeer = Core.GetService<DiscoveryService>().GetPeerInfoByIpAddress(Ip)!;
            Core.SendEvent(new PeerStartControlingEvent() { PeerInfo = receivedPeer });
        }
    }
}
