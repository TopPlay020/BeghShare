namespace BeghShare.Core.Events.UserInputEvents
{
    public record MouseMoveEvent
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
