using BeghShare.Core.Models;
namespace BeghShare.Core.Events.MessageEvents
{
    public record PeerStartControlingEvent
    {
        public PeerInfo PeerInfo { get; set; }
    }
}
