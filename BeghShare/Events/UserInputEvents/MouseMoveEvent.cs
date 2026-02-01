using SharpHook;

namespace BeghShare.Events.UserInputEvents
{
    public record MouseMoveEvent
    {
        public required int X { get; init; }
        public required int Y { get; init; }
    }
}
