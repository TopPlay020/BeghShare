using SharpHook.Data;

namespace BeghShare.Events.UserInputEvents
{
    public record KeyReleasedEvent
    {
        public required KeyCode keyCode { get; init; }
    }
}
