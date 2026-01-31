using BeghShare.Models;
namespace BeghShare.Events
{
    public record PeerStartControlingEvent
    {
        public required PeerInfo PeerInfo { get; init; }
    }
}
