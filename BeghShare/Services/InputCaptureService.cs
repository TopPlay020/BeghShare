using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
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
            _hook.KeyPressed += OnKeyDown;
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

        private void OnKeyDown(object? sender, KeyboardHookEventArgs e)
        {
            Core.SendEvent(new KeyDownEvent() { keyCode = e.Data.KeyCode });
        }
    }
}