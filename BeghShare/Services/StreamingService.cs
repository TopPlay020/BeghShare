using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
using System.Diagnostics;
using System.Text;

namespace BeghShare.Services
{
    public class StreamingService : ISingleton, IGUIAutoStart
    {
        private const string SendPeerControlRequestMsg = "BeghSendPeerControlRequestEvent:";
        private const string SendPeerControlResponseMsg = "BeghSendPeerControlResponseEvent:";

        [EventHandler]
        public async void OnSendPeerControlRequestEvent(SendPeerControlRequestEvent e)
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(SendPeerControlRequestMsg);
            Core.SendEvent(new UdpMsgSendEvent()
            {
                Data = sendBytes,
                RemoteEndPoint = e.PeerInfo.IPEndPoint
            });
        }

        [EventHandler]
        public async void OnUdpMsgReceived(UdpMsgReceivedEvent e)
        {
            string msg = Encoding.UTF8.GetString(e.Data);
            if (msg.StartsWith(SendPeerControlRequestMsg))
            {
                var senderPeer = Core.GetService<DiscoveryService>().Peers.FirstOrDefault(p => p.IP.ToString() == e.RemoteEndPoint.Address.ToString());
                if (senderPeer == null)
                {
                    //TODO: I need To Notify DiscoveryService that there is new Ip Addresse !!!!
                }
                var result = MessageBox.Show(
                $"{senderPeer!.Name}:{e.RemoteEndPoint.Address} Want To Control You",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            }
        }
    }
}
