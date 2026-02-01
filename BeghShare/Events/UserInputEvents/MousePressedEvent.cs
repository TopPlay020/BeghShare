using SharpHook.Data;
namespace BeghShare.Events.UserInputEvents
{
    public record MousePressedEvent
    {
        public required MouseButton Button { get;init; }
    }
}
