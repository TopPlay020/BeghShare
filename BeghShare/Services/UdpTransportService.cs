using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace BeghShare.Services
{
    public class UdpTransportService : ISingleton, IGUIAutoStart
    {
        public const int APPPORT_UDP = 51354;

        private readonly UdpClient _udpClient;

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
                    _ = Task.Run(() => Core.SendEvent(new UdpMsgReceivedEvent(result)));
                }
            });
        }

        [EventHandler]
        public async void OnUdpMsgSend(UdpMsgSendEvent e)
        {
            await _udpClient.SendAsync(e.Data, e.Data.Length, e.RemoteEndPoint);
        }
    }
}
