using BeghShare.Core.Models;

namespace BeghShare.Core.Events.MessageEvents
{
    public record SendPeerControlRequestEvent
    {
        public PeerInfo PeerInfo { get; set; }
        public SendPeerControlRequestEvent(PeerInfo peerInfo)
        {
            this.PeerInfo = peerInfo;
        }
    }
}
