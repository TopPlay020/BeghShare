using SharpHook;

namespace BeghShare.Events
{
    public record MouseMoveEvent
    {
        public required int X { get; init; }
        public required int Y { get; init; }
    }
}
