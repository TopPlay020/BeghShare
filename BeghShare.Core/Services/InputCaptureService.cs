using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Events;
using BeghShare.Core.Events.MessageEvents;
using BeghShare.Core.Events.UserInputEvents;
using SharpHook;
using SharpHook.Data;

namespace BeghShare.Core.Services
{
    public class InputCaptureService : ISingleton, IAutoStart
    {
        private SimpleGlobalHook _hook;

        private bool _suppressEvent = false;

        [EventHandler]
        public void OnStartControling(PeerStartControlingEvent _)
        {
            Task.Run(StartTracking);
        }

        [EventHandler]
        public async void StartControledBy(PeerControlMeEvent e)
        {
            StopTracking();
        }

        public void StartTracking()
        {
            if (_hook != null)
                return;

            _hook = new SimpleGlobalHook(GlobalHookType.Keyboard | GlobalHookType.Mouse, runAsyncOnBackgroundThread: true);
            _hook.MouseMoved += OnMouseMove;
            _hook.MouseDragged += OnMouseMove;
            _hook.MouseWheel += OnMouseWheel;
            _hook.MousePressed += OnMousePressed;
            _hook.MouseReleased += OnMouseReleased;
            _hook.KeyPressed += OnKeyPressed;
            _hook.KeyReleased += OnKeyReleased;
            _hook.RunAsync();
        }

        public void StopTracking()
        {
            _hook?.Dispose();
            _hook = null;
        }

        [EventHandler]
        public void OnMouseExitOSEvent(MouseExitOSEvent _)
        {
            _suppressEvent = true;
        }

        [EventHandler]
        public void OnMouseEnterOSEvent(MouseEnterOSEvent _)
        {
            _suppressEvent = false;
        }

        [EventHandler]
        public void OnApplicationExit(MainWindowCloseEvent _)
        {
            StopTracking();
        }

        private void OnMouseMove(object sender, MouseHookEventArgs e)
        {
            SendEvent(new MouseMoveEvent()
            {
                X = e.Data.X,
                Y = e.Data.Y
            });
            if (_suppressEvent)
                e.SuppressEvent = true;
        }
        private void OnMouseWheel(object sender, MouseWheelHookEventArgs e)
        {
            SendEvent(new MouseWheelEvent()
            {
                Rotation = e.Data.Rotation,
                Direction = e.Data.Direction,
                ScrollType = e.Data.Type
            });
            if (_suppressEvent)
                e.SuppressEvent = true;
        }
        private void OnMousePressed(object sender, MouseHookEventArgs e)
        {
            SendEvent(new MousePressedEvent() { Button = e.Data.Button });
            if (_suppressEvent)
                e.SuppressEvent = true;
        }
        private void OnMouseReleased(object sender, MouseHookEventArgs e)
        {
            SendEvent(new MouseReleasedEvent() { Button = e.Data.Button });
            if (_suppressEvent)
                e.SuppressEvent = true;
        }
        private void OnKeyPressed(object sender, KeyboardHookEventArgs e)
        {
            SendEvent(new KeyPressedEvent() { keyCode = e.Data.KeyCode });
            if (_suppressEvent)
                e.SuppressEvent = true;
        }
        private void OnKeyReleased(object sender, KeyboardHookEventArgs e)
        {
            SendEvent(new KeyReleasedEvent() { keyCode = e.Data.KeyCode });
            if (_suppressEvent)
                e.SuppressEvent = true;
        }
    }
}