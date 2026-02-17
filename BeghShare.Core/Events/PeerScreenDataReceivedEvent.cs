using BeghShare.Core.Models;

namespace BeghShare.Core.Events
{
    public class PeerScreenDataReceivedEvent
    {
        public PeerInfo peerInfo { get; set; }
        public PeerScreenData peerScreenData { get; set; }
    }
}
