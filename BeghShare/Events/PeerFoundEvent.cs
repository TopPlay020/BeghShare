using BeghShare.Models;

namespace BeghShare.Events
{
    public class PeerFoundEvent
    {
        public PeerInfo PeerInfo { get; set; }
        public PeerFoundEvent(PeerInfo peerInfo)
        {
            this.PeerInfo = peerInfo;
        }
    }
}
