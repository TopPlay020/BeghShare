using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Attributes;
using BeghShare.Core.Events;
using BeghShare.Core.Events.MessageEvents;
using BeghShare.Core.Events.NetworkEvents;
using BeghShare.Core.Events.UserInputEvents;
using BeghShare.Core.Models;
using System.Net;

namespace BeghShare.Core.Services
{
    public class ScreenManagementService : ISingleton, IAutoStart
    {
        private const string SendScreenResolutionMsg = "SendScreenResolutionEvent:";
        private const string SendMouseEnterOSMsg = "SendMouseEnterOSEvent:";

        private readonly int ScreenWidth;
        private readonly int ScreenHeight;

        private readonly Dictionary<PeerInfo, PeerScreenData> PeerScreenDataMap = [];

        public enum MonitorPosition
        {
            Left,
            Right,
            Top,
            Bottom
        }

        private readonly List<PeerInfo> InTheLeft = [];
        private readonly List<PeerInfo> InTheRight = [];
        private readonly List<PeerInfo> InTheTop = [];
        private readonly List<PeerInfo> InTheBottom = [];

        public ScreenManagementService()
        {
            var (Width, Height) = GetService<IScreenService>().GetResolution();
            ScreenWidth = Width;
            ScreenHeight = Height;
        }

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
            string data = $"{ScreenWidth}/{ScreenHeight}";
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
        public async void OnMouseMoveEvent(MouseMoveEvent e)
        {
            if (e.X > ScreenWidth)
                OnMouseExitScreenEvent(MonitorPosition.Right);
            else if (e.X < 0)
                OnMouseExitScreenEvent(MonitorPosition.Left);
            else if (e.Y > ScreenHeight)
                OnMouseExitScreenEvent(MonitorPosition.Bottom);
            else if (e.Y < 0)
                OnMouseExitScreenEvent(MonitorPosition.Top);
        }

        public void SetPeerPosition(PeerInfo peerInfo, MonitorPosition position)
        {
            switch (position)
            {
                case MonitorPosition.Left: InTheLeft.Add(peerInfo); break;
                case MonitorPosition.Right: InTheRight.Add(peerInfo); break;
                case MonitorPosition.Top: InTheTop.Add(peerInfo); break;
                case MonitorPosition.Bottom: InTheBottom.Add(peerInfo); break;
            }

        }

        public void OnMouseExitScreenEvent(MonitorPosition position)
        {
            var list = position switch
            {
                MonitorPosition.Left => InTheLeft,
                MonitorPosition.Right => InTheRight,
                MonitorPosition.Top => InTheTop,
                MonitorPosition.Bottom => InTheBottom,
                _ => throw new Exception("Invalid Exit Side")
            };

            var peerInfo = list.FirstOrDefault();

            if (peerInfo == null) return;

            SendEvent(new MouseExitOSEvent());
            SendEvent(new TcpMsgSendEvent()
            {
                Header = SendMouseEnterOSMsg,
                Data = string.Empty,
                Ip = peerInfo.IP
            });
        }
        [MsgEventHandler(SendMouseEnterOSMsg)]
        public void OnMouseEnterOSMsg(string _, IPAddress __)
        {
            SendEvent(new MouseEnterOSEvent());
        }
    }
}
