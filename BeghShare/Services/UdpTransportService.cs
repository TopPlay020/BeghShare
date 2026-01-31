using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace BeghShare.Services
{
    public class UdpTransportService : ISingleton, IAutoStart
    {
        public const int APPPORT_UDP = 51354;

        private readonly UdpClient _udpClient;
        private readonly ConcurrentDictionary<IPAddress, IPEndPoint> _ipDict = new();

        public UdpTransportService()
        {
            _udpClient = new UdpClient(APPPORT_UDP) { EnableBroadcast = true };
            StartListening();
        }

        private void StartListening()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var result = await _udpClient.ReceiveAsync();
                    var data = EncryptionService.Decode(result.Buffer);
                    _ = Task.Run(() => Core.SendEvent(new UdpMsgReceivedEvent
                    {
                        Data = data,
                        Ip = result.RemoteEndPoint.Address
                    }));
                }
            });
        }

        [EventHandler]
        public async void OnUdpMsgSend(UdpMsgSendEvent e)
        {
            var bytes = EncryptionService.Encode(e.Data);
            await _udpClient.SendAsync(bytes, bytes.Length, _ipDict.GetOrAdd(e.Ip, new IPEndPoint(e.Ip, APPPORT_UDP)));
        }
    }
}
