using BeghCore;
using BeghCore.Attributes;
using BeghShare.Core.Events;
using BeghShare.Core.Events.MessageEvents;
using BeghShare.Core.Events.UserInputEvents;

namespace BeghShare.Core.Services
{
    public class ControlStateService : ISingleton, IAutoStart
    {
        private int ScreenWidth;
        private int ScreenHeight;

        public ControlStateService()
        {
            var (Width, Height) = GetService<IScreenService>().GetResolution();
            ScreenWidth = Width;
            ScreenHeight = Height;
        }

        [EventHandler]
        public async void OnMouseMoveEvent(MouseMoveEvent e)
        {
            if (e.X > ScreenWidth)
                SendEvent(new MouseExitScreenEvent() { ExitSide = MouseExitScreenEvent.RIGHT, ExitPosition = e.Y });
            else if (e.X < 0)
                SendEvent(new MouseExitScreenEvent() { ExitSide = MouseExitScreenEvent.LEFT, ExitPosition = e.Y });
            else if (e.Y > ScreenHeight)
                SendEvent(new MouseExitScreenEvent() { ExitSide = MouseExitScreenEvent.DOWN, ExitPosition = e.X });
            else if (e.Y < 0)
                SendEvent(new MouseExitScreenEvent() { ExitSide = MouseExitScreenEvent.UP, ExitPosition = e.X });
        }

        [EventHandler]
        public async void OnMouseExitOSEvent(MouseExitOSEvent e)
        {
            //TODO: Implement logic for when the mouse exits the operating system
        }

        [EventHandler]
        public async void OnMouseEnterOSEvent(MouseEnterOSEvent e)
        {
            //TODO: Implement logic for when the mouse enters the operating system
        }
    }
}
