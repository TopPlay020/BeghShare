namespace BeghShare.Core.Events
{
    public record YesNoQustionResponseEvent
    {
        public string RequestId { get; set; }
        public bool Response { get; set; }
    }
}
