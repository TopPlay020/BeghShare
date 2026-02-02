namespace BeghShare.Core.Events
{
    public record YesNoQustionRequestEvent
    {
        public string RequestId { get; set; }
        public string RequestTitle { get; set; }
        public string RequestBody { get; set; }
    }
}
