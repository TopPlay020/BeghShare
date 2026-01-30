using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
using Gma.System.MouseKeyHook;

namespace BeghShare.Services
{
    public class InputCaptureService : ISingleton, IGUIAutoStart
    {
        private static IKeyboardMouseEvents? _globalHook;

        public InputCaptureService()
        {
            //StartTracking();
        }
        public void StartTracking()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.MouseMove += MouseMove;
            _globalHook.KeyDown += KeyDown;
        }
        public void StopTracking()
        {
            _globalHook?.MouseMove -= MouseMove;
            _globalHook?.KeyDown -= KeyDown;
            _globalHook?.Dispose();
        }
        [EventHandler]
        public void OnApplicationExit(MainWindowCloseEvent e)
        {
            StopTracking();
        }

        public void MouseMove(object? sender, MouseEventArgs e)
        {
            Core.SendEvent(new MouseMoveEvent(e));
        }

        public void KeyDown(object? sender, KeyEventArgs e)
        {
            Core.SendEvent(new KeyDownEvent(e));
        }
    }
}
