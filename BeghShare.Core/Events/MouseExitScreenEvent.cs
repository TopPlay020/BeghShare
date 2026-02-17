namespace BeghShare.Core.Events
{
    public class MouseExitScreenEvent
    {
        public const int LEFT = 0;
        public const int RIGHT = 1;
        public const int UP = 2;
        public const int DOWN = 3;
        public int ExitSide { get; set; }
        public int ExitPosition { get; set; }
    }
}
