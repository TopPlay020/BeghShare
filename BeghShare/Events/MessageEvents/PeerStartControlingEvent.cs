using BeghShare.Models;
namespace BeghShare.Events.MessageEvents
{
    public record PeerStartControlingEvent
    {
        public required PeerInfo PeerInfo { get; init; }
    }
}
