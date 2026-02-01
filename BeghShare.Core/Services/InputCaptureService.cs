using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Events;
using BeghShare.Core.Events.MessageEvents;
using BeghShare.Core.Events.UserInputEvents;
using SharpHook;

namespace BeghShare.Core.Services
{
    public class InputCaptureService : ISingleton, IAutoStart
    {
        private SimpleGlobalHook _hook;

        [EventHandler]
        public void OnStartControling(PeerStartControlingEvent _)
        {
            Task.Run(StartTracking);
        }

        public void StartTracking()
        {
            if (_hook != null)
                return;

            _hook = new SimpleGlobalHook();
            _hook.MouseMoved += OnMouseMove;
            _hook.MousePressed += OnMousePressed;
            _hook.MouseReleased += OnMouseReleased;
            _hook.KeyPressed += OnKeyPressed;
            _hook.KeyReleased += OnKeyReleased;
            _hook.Run();
        }

        public void StopTracking()
        {
            _hook?.Dispose();
            _hook = null;
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
        }
        private void OnMousePressed(object sender, MouseHookEventArgs e)
        {
            SendEvent(new MousePressedEvent() { Button = e.Data.Button });
        }
        private void OnMouseReleased(object sender, MouseHookEventArgs e)
        {
            SendEvent(new MouseReleasedEvent() { Button = e.Data.Button });
        }
        private void OnKeyPressed(object sender, KeyboardHookEventArgs e)
        {
            SendEvent(new KeyPressedEvent() { keyCode = e.Data.KeyCode });
        }
        private void OnKeyReleased(object sender, KeyboardHookEventArgs e)
        {
            SendEvent(new KeyReleasedEvent() { keyCode = e.Data.KeyCode });
        }
    }
}