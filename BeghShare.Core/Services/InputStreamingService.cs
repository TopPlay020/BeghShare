using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Attributes;
using BeghShare.Core.Events.MessageEvents;
using BeghShare.Core.Events.NetworkEvents;
using BeghShare.Core.Events.UserInputEvents;
using BeghShare.Core.Models;
using SharpHook.Data;
using System.Net;

namespace BeghShare.Core.Services
{
    public class InputStreamingService : ISingleton, IAutoStart
    {
        private const string MOUSE_MOVE_MSG = "MouseMoveEvent:";
        private const string MOUSE_WHEEL_MSG = "MouseWheelEvent:";
        private const string MOUSE_PRESSED_EVENT_MSG = "MousePressedEvent:";
        private const string MOUSE_RELEASED_EVENT_MSG = "MouseReleasedEvent:";
        private const string KEY_PRESSED_EVENT_MSG = "KeyPressedEvent:";
        private const string KEY_RELEASED_EVENT_MSG = "KeyReleasedEvent:";

        public PeerInfo ControledBy;
        public PeerInfo Controled;

        private InputSimulatorService _simulator;

        [EventHandler]
        public async void StartControledBy(PeerControlMeEvent e)
        {
            ControledBy = e.PeerInfo;
            _simulator = GetService<InputSimulatorService>();
        }

        [EventHandler]
        public void StartControling(PeerStartControlingEvent e)
        {
            Controled = e.PeerInfo;
        }

        [EventHandler]
        public async void OnMouseMoveEvent(MouseMoveEvent e)
        {
            if (Controled == null) return;
            SendEvent(new UdpMsgSendEvent()
            {
                Header = MOUSE_MOVE_MSG,
                Data = $"{e.X}/{e.Y}",
                Ip = Controled.IP
            });
        }
        [EventHandler]
        public async void OnMouseWheelEvent(MouseWheelEvent e)
        {
            if (Controled == null) return;
            SendEvent(new UdpMsgSendEvent()
            {
                Header = MOUSE_WHEEL_MSG,
                Data = $"{e.Rotation}/{e.Direction}/{e.ScrollType}",
                Ip = Controled.IP
            });
        }
        [EventHandler]
        public async void OnMousePressedEvent(MousePressedEvent e)
        {
            if (Controled == null) return;
            SendEvent(new UdpMsgSendEvent()
            {
                Header = MOUSE_PRESSED_EVENT_MSG,
                Data = $"{e.Button}",
                Ip = Controled.IP
            });
        }
        [EventHandler]
        public async void OnMouseReleasedEvent(MouseReleasedEvent e)
        {
            if (Controled == null) return;
            SendEvent(new UdpMsgSendEvent()
            {
                Header = MOUSE_RELEASED_EVENT_MSG,
                Data = $"{e.Button}",
                Ip = Controled.IP
            });
        }

        [EventHandler]
        public async void OnKeyPressedEvent(KeyPressedEvent e)
        {
            if (Controled == null) return;
            SendEvent(new UdpMsgSendEvent()
            {
                Header = KEY_PRESSED_EVENT_MSG,
                Data = $"{e.keyCode}",
                Ip = Controled.IP
            });
        }
        [EventHandler]
        public async void OnKeyReleasedEvent(KeyReleasedEvent e)
        {
            if (Controled == null) return;
            SendEvent(new UdpMsgSendEvent()
            {
                Header = KEY_RELEASED_EVENT_MSG,
                Data = $"{e.keyCode}",
                Ip = Controled.IP
            });
        }

        [MsgEventHandler(MOUSE_MOVE_MSG)]
        public async void OnMouseMoveFromController(string data, IPAddress Ip)
        {
            if (!Ip.Equals(ControledBy?.IP)) return;

            string[] parts = data.Split('/');
            var x = short.Parse(parts[0]);
            var y = short.Parse(parts[1]);

            SendEvent(new MouseMoveEvent() { X = x, Y = y });
            _simulator.SimulateMouseMovement(x, y);
        }

        [MsgEventHandler(MOUSE_WHEEL_MSG)]
        public async void OnMouseWheelFromController(string data, IPAddress Ip)
        {
            if (!Ip.Equals(ControledBy?.IP)) return;

            string[] parts = data.Split('/');
            var rotation = short.Parse(parts[0]);
            var direction = (MouseWheelScrollDirection)Enum.Parse(typeof(MouseWheelScrollDirection), parts[1]);
            var scrollType = (MouseWheelScrollType)Enum.Parse(typeof(MouseWheelScrollType), parts[2]);
            SendEvent(new MouseWheelEvent() { Rotation = rotation, Direction = direction, ScrollType = scrollType });
            _simulator.SimulateMouseWheel(rotation, direction, scrollType);
        }
        [MsgEventHandler(MOUSE_PRESSED_EVENT_MSG)]
        public async void OnMousePressedFromController(string data, IPAddress Ip)
        {
            if (!Ip.Equals(ControledBy?.IP)) return;

            var button = (MouseButton)Enum.Parse(typeof(MouseButton), data);
            SendEvent(new MousePressedEvent() { Button = button });
            _simulator.SimulateMousePress(button);
        }

        [MsgEventHandler(MOUSE_RELEASED_EVENT_MSG)]
        public async void OnMouseReleasedFromController(string data, IPAddress Ip)
        {
            if (!Ip.Equals(ControledBy?.IP)) return;

            var button = (MouseButton)Enum.Parse(typeof(MouseButton), data);
            SendEvent(new MouseReleasedEvent() { Button = button });
            _simulator.SimulateMouseRelease(button);
        }

        [MsgEventHandler(KEY_PRESSED_EVENT_MSG)]
        public async void OnKeyPressedEventFromController(string data, IPAddress Ip)
        {
            if (!Ip.Equals(ControledBy?.IP)) return;

            var keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), data);
            SendEvent(new KeyPressedEvent() { keyCode = keyCode });
            _simulator.SimulateKeyPress(keyCode);
        }

        [MsgEventHandler(KEY_RELEASED_EVENT_MSG)]
        public async void OnKeyReleasedEventFromController(string data, IPAddress Ip)
        {
            if (!Ip.Equals(ControledBy?.IP)) return;

            var keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), data);
            SendEvent(new KeyReleasedEvent() { keyCode = keyCode });
            _simulator.SimulateKeyRelease(keyCode);
        }
    }
}