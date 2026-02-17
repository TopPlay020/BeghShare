using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Attributes;
using BeghShare.Core.Events;
using BeghShare.Core.Events.MessageEvents;
using BeghShare.Core.Events.NetworkEvents;
using BeghShare.Core.Models;
using System.Net;

namespace BeghShare.Core.Services
{
    //TODO: Fill This Class
    //this class is the one responsible switching the screen when the user is controlling another peer,
    //and also for switching back to the main screen when the user stops controlling another peer.
    //It should also be responsible for keeping track of which screen is currently active, and for notifying other services when the screen changes.
    public class ScreenManagementService : ISingleton, IAutoStart
    {
        private const string SendScreenResolutionMsg = "SendScreenResolutionEvent:";

        private readonly Dictionary<PeerInfo, PeerScreenData> PeerScreenDataMap = [];

        [EventHandler]
        public async void StartControledBy(PeerControlMeEvent e)
        {
            SendResolutionTo(e.PeerInfo.IP);
        }

        [EventHandler]
        public void StartControling(PeerStartControlingEvent e)
        {
            SendResolutionTo(e.PeerInfo.IP);
        }

        private void SendResolutionTo(IPAddress Ip)
        {
            var (Width, Height) = GetService<IScreenService>().GetResolution();
            string data = $"{Width}/{Height}";
            SendEvent(new TcpMsgSendEvent
            {
                Header = SendScreenResolutionMsg,
                Data = data,
                Ip = Ip
            });
        }

        [MsgEventHandler(SendScreenResolutionMsg)]
        public async void OnSendScreenResolutionMsg(string data, IPAddress Ip)
        {
            var peerInfo = GetService<DiscoveryService>().GetPeerInfoByIpAddress(Ip);
            string[] parts = data.Split('/');
            var Width = int.Parse(parts[0]);
            var Height = int.Parse(parts[1]);
            PeerScreenDataMap[peerInfo] = new PeerScreenData
            {
                Width = Width,
                Height = Height
            };
            SendEvent(new PeerScreenDataReceivedEvent()
            {
                peerInfo = peerInfo,
                peerScreenData = PeerScreenDataMap[peerInfo]
            });
        }

        public List<(PeerInfo peerInfo, PeerScreenData peerScreenData)> GetPeerScreenData()
        {
            return [.. PeerScreenDataMap.Select(x => (x.Key, x.Value))];
        }

        [EventHandler]
        public void OnMouseExitScreenEvent(MouseExitScreenEvent e)
        {

        }
    }
}
