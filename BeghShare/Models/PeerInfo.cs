using System.Net;

namespace BeghShare.Models
{
    public record PeerInfo
    {
        public required IPAddress IP { get; init; }
        public required string Name { get; init; }
        public required bool IsOnline { get; init; }

        public required IPEndPoint IPEndPoint { get; init; }

    }
}
