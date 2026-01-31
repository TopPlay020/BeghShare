using BeghCore;
using BeghCore.Attributes;
using BeghShare.Attributes;
using BeghShare.Events;
using BeghShare.Models;
using SharpHook;
using SharpHook.Data;
using System.Net;

namespace BeghShare.Services
{
    public class InputInjectorService : ISingleton, IAutoStart
    {
        private const string MOUSEMOVE_MSG = "MouseMoveEvent:";
        private const string KEYDOWNEVENT_MSG = "KeyDownEvent:";

        public PeerInfo? ControledBy;
        public PeerInfo? Controled;

        private EventSimulator? _simulator;

        [EventHandler]
        public async void StartControledBy(PeerControlMeEvent e)
        {
            ControledBy = e.PeerInfo;
            _simulator = new EventSimulator();
        }

        [EventHandler]
        public void StartControling(PeerStartControlingEvent e)
        {
            Controled = e.PeerInfo;
        }

        [EventHandler]
        public async void OnMouseMoveEvent(MouseMoveEvent e)
        {
            if (Controled == null)
                return;
            Core.SendEvent(new UdpMsgSendEvent()
            {
                Data = $"{MOUSEMOVE_MSG}{e.X}/{e.Y}",
                RemoteEndPoint = Controled.IPEndPoint
            });
        }

        [EventHandler]
        public async void OnKeyDownEvent(KeyDownEvent e)
        {
            if (Controled == null) return;
            Core.SendEvent(new UdpMsgSendEvent()
            {
                Data = $"{KEYDOWNEVENT_MSG}{e.keyCode}",
                RemoteEndPoint = Controled.IPEndPoint
            });
        }

        [MsgEventHandler(MOUSEMOVE_MSG)]
        public async void OnMouseMoveFromController(string data, IPEndPoint remoteEndPoint)
        {
            if (remoteEndPoint.Address.ToString() != ControledBy?.IP.ToString()) return;

            string[] parts = data.Split('/');
            var x = short.Parse(parts[0]);
            var y = short.Parse(parts[1]);

            Core.SendEvent(new MouseMoveEvent() { X = x, Y = y });
            _simulator?.SimulateMouseMovement(x, y);
        }

        [MsgEventHandler(KEYDOWNEVENT_MSG)]
        public async void OnKeyDownEventFromController(string data, IPEndPoint remoteEndPoint)
        {
            if (remoteEndPoint.Address.ToString() != ControledBy?.IP.ToString()) return;

            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), data);
            Core.SendEvent(new KeyDownEvent() { keyCode = keyCode });
            _simulator?.SimulateKeyPress(keyCode);
        }
    }
}