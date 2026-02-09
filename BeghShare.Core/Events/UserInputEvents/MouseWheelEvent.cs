using SharpHook.Data;

namespace BeghShare.Core.Events.UserInputEvents
{
    public class MouseWheelEvent
    {
        public int Rotation { get; set; }
        public MouseWheelScrollDirection Direction { get; set; }
        public MouseWheelScrollType ScrollType { get; set; }
    }
}
