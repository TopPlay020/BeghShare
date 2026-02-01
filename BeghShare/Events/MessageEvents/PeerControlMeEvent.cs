using BeghShare.Models;
namespace BeghShare.Events.MessageEvents
{
    public record PeerControlMeEvent
    {
        public required PeerInfo PeerInfo { get; init; }
    }
}
