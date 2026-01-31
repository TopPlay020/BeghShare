using BeghCore;
using BeghCore.Attributes;
using BeghShare.Events;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace BeghShare.Services
{
    public class TcpTransportService : ISingleton, IAutoStart
    {
        public const int APPPORT_TCP = 51355;

        private readonly TcpListener _listener;
        private readonly ConcurrentDictionary<IPEndPoint, TcpClient> _clients = new();

        public TcpTransportService()
        {
            _listener = new TcpListener(IPAddress.Any, APPPORT_TCP);
            StartListening();
        }

        private void StartListening()
        {
            _listener.Start();
            Task.Run(AcceptLoop);
        }

        private async Task AcceptLoop()
        {
            while (true)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    var remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint!;
                    _clients[remoteEndPoint] = client;
                    _ = Task.Run(() => ReceiveLoop(client, remoteEndPoint));
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception)
                {
                    // Log if needed
                }
            }
        }

        private async Task ReceiveLoop(TcpClient client, IPEndPoint remoteEndPoint)
        {
            try
            {
                await using var stream = client.GetStream();
                var lengthBuffer = new byte[4];

                while (true)
                {
                    if (await ReadExactlyAsync(stream, lengthBuffer, 4) == 0)
                        break;

                    var length = BitConverter.ToInt32(lengthBuffer, 0);
                    if (length <= 0 || length > 1024 * 1024) // 1MB max
                        break;

                    var dataBuffer = new byte[length];
                    if (await ReadExactlyAsync(stream, dataBuffer, length) == 0)
                        break;

                    var data = EncryptionService.Decode(dataBuffer);
                    if (!string.IsNullOrEmpty(data))
                    {
                        _ = Task.Run(() => Core.SendEvent(new TcpMsgReceivedEvent
                        {
                            Data = data,
                            RemoteEndPoint = remoteEndPoint
                        }));
                    }
                }
            }
            catch (Exception)
            {
                // Connection closed or error
            }
            finally
            {
                _clients.TryRemove(remoteEndPoint, out _);
                client.Dispose();
            }
        }

        private static async Task<int> ReadExactlyAsync(Stream stream, byte[] buffer, int count)
        {
            var total = 0;
            while (total < count)
            {
                var read = await stream.ReadAsync(buffer.AsMemory(total, count - total));
                if (read == 0)
                    return total;
                total += read;
            }
            return total;
        }

        [EventHandler]
        public async void OnTcpMsgSend(TcpMsgSendEvent e)
        {
            if (!_clients.TryGetValue(e.RemoteEndPoint, out var client) || !client.Connected)
                return;

            var bytes = EncryptionService.Encode(e.Data);
            var lengthBytes = BitConverter.GetBytes(bytes.Length);
            var stream = client.GetStream();
            await stream.WriteAsync(lengthBytes);
            await stream.WriteAsync(bytes);
        }
    }
}
