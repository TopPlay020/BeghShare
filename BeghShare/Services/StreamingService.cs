using BeghCore;
using BeghCore.Attributes;
using BeghShare.Attributes;
using BeghShare.Events;
using System.Net;

namespace BeghShare.Services
{
    //TODO: sending change to TCP / Make Sure the Msg Recived
    public class StreamingService : ISingleton, IAutoStart
    {
        private const string SendPeerControlRequestMsg = "BeghSendPeerControlRequestEvent:";
        private const string SendPeerControlAcceptMsg = "BeghSendPeerControlAcceptEvent:";

        [EventHandler]
        public async void OnSendPeerControlRequestEvent(SendPeerControlRequestEvent e)
        {
            Core.SendEvent(new UdpMsgSendEvent
            {
                Data = SendPeerControlRequestMsg,
                RemoteEndPoint = e.PeerInfo.IPEndPoint
            });
        }

        [MsgEventHandler(SendPeerControlRequestMsg)]
        public async void OnSendPeerControlRequest(string data, IPEndPoint remoteEndPoint)
        {
            var senderPeer = Core.GetService<DiscoveryService>().Peers.FirstOrDefault(p => p.IP.ToString() == remoteEndPoint.Address.ToString());
            var senderName = senderPeer != null ? senderPeer.Name : "Unknown";
            if (senderPeer == null)
                Core.GetService<DiscoveryService>().Discover(remoteEndPoint.Address.ToString());
            var mainWindow = Core.GetService<MainWindow>();
            mainWindow.Invoke((MethodInvoker)(() =>
            {
                var result = MessageBox.Show(
                    mainWindow,
                    $"{senderName}:{remoteEndPoint.Address} Want To Control You",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Core.SendEvent(new UdpMsgSendEvent { Data = SendPeerControlAcceptMsg, RemoteEndPoint = remoteEndPoint });
                    if (senderPeer == null)
                        senderPeer = Core.GetService<DiscoveryService>().Peers.First(p => p.IP.ToString() == remoteEndPoint.Address.ToString());
                    Core.SendEvent(new PeerControlMeEvent() { PeerInfo = senderPeer });
                }
            }));
        }

        [MsgEventHandler(SendPeerControlAcceptMsg)]
        public async void OnSendPeerControlAcceptMsg(string data, IPEndPoint remoteEndPoint)
        {
            var receivedPeer = Core.GetService<DiscoveryService>().Peers.First(p => p.IP.ToString() == remoteEndPoint.Address.ToString());
            Core.SendEvent(new PeerStartControlingEvent() { PeerInfo = receivedPeer });
        }
    }
}
