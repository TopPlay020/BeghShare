using BeghShare.Core.Models;

namespace BeghShare.Core.Events.MessageEvents
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
