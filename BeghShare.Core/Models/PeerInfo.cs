using System.Net;

namespace BeghShare.Core.Models
{
    public record PeerInfo
    {
        public IPAddress IP { get; set; }
        public string Name { get; set; }
    }
}
