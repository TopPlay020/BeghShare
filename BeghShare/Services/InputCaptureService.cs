using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
using BeghShare.Events.MessageEvents;
using BeghShare.Events.UserInputEvents;
using SharpHook;

namespace BeghShare.Services
{
    public class InputCaptureService : ISingleton, IAutoStart
    {
        private SimpleGlobalHook? _hook;

        [EventHandler]
        public void OnStartControling(PeerStartControlingEvent e)
        {
            var _ = Task.Run(StartTracking);
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
            if (_hook != null)
            {
                _hook.Dispose();
                _hook = null;
            }
        }

        [EventHandler]
        public void OnApplicationExit(MainWindowCloseEvent e)
        {
            StopTracking();
        }

        private void OnMouseMove(object? sender, MouseHookEventArgs e)
        {
            Core.SendEvent(new MouseMoveEvent()
            {
                X = e.Data.X,
                Y = e.Data.Y
            });
        }
        private void OnMousePressed(object? sender, MouseHookEventArgs e)
        {
            Core.SendEvent(new MousePressedEvent() { Button = e.Data.Button });
        }
        private void OnMouseReleased(object? sender, MouseHookEventArgs e)
        {
            Core.SendEvent(new MouseReleasedEvent() { Button = e.Data.Button });
        }
        private void OnKeyPressed(object? sender, KeyboardHookEventArgs e)
        {
            Core.SendEvent(new KeyPressedEvent() { keyCode = e.Data.KeyCode });
        }
        private void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
        {
            Core.SendEvent(new KeyReleasedEvent() { keyCode = e.Data.KeyCode });
        }
    }
}