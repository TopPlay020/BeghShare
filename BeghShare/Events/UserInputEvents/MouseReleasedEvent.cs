using SharpHook.Data;
namespace BeghShare.Events.UserInputEvents
{
    public record MouseReleasedEvent
    {
        public required MouseButton Button { get; init; }
    }
}
