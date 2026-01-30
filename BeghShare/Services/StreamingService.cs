using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;

namespace BeghShare.Services
{
    public class StreamingService : ISingleton, IGUIAutoStart
    {
        private const string SendPeerControlRequestMsg = "BeghSendPeerControlRequestEvent:";
        private const string SendPeerControlResponseMsg = "BeghSendPeerControlResponseEvent:";

        [EventHandler]
        public async void OnSendPeerControlRequestEvent(SendPeerControlRequestEvent e)
        {
            Core.SendEvent(new UdpMsgSendEvent
            {
                Data = SendPeerControlRequestMsg,
                RemoteEndPoint = e.PeerInfo.IPEndPoint
            });
        }

        [EventHandler]
        public async void OnUdpMsgReceived(UdpMsgReceivedEvent e)
        {
            if (e.Data.StartsWith(SendPeerControlRequestMsg))
            {
                var senderPeer = Core.GetService<DiscoveryService>().Peers.FirstOrDefault(p => p.IP.ToString() == e.RemoteEndPoint.Address.ToString());
                if (senderPeer == null)
                {
                    //TODO: I need To Notify DiscoveryService that there is new Ip Addresse !!!!
                }
                var mainWindow = Core.GetService<MainWindow>();
                var result = MessageBox.Show(
                mainWindow,
                $"{senderPeer!.Name}:{e.RemoteEndPoint.Address} Want To Control You",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            }
        }
    }
}
