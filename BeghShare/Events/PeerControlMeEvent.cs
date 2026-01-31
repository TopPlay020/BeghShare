using BeghShare.Models;
namespace BeghShare.Events
{
    public record PeerControlMeEvent
    {
        public required PeerInfo PeerInfo { get; init; }
    }
}
