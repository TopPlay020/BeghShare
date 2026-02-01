using BeghShare.Core.Models;
namespace BeghShare.Core.Events.MessageEvents
{
    public record PeerControlMeEvent
    {
        public PeerInfo PeerInfo { get; set; }
    }
}
