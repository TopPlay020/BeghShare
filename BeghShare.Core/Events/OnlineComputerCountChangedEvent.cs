namespace BeghShare.Core.Events
{
    public class OnlineComputerCountChangedEvent
    {
        public int Count { get; }
        public OnlineComputerCountChangedEvent(int count)
        {
            Count = count;
        }
    }
}
