using SharpHook.Data;

namespace BeghShare.Core.Events.UserInputEvents
{
    public record KeyReleasedEvent
    {
        public KeyCode keyCode { get; set; }
    }
}
