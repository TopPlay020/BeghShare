using BeghShare.Models;

namespace BeghShare.Events
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
