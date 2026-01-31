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
        private readonly ConcurrentDictionary<IPAddress, IPEndPoint> _ipDict = new();

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
                    var peerInfo = Core.GetService<DiscoveryService>().GetPeerInfoByIpAddress(remoteEndPoint.Address);
                    if (peerInfo != null)
                    {
                        _clients[remoteEndPoint] = client;
                        _ipDict[remoteEndPoint.Address] = remoteEndPoint;
                        _ = Task.Run(() => ReceiveLoop(client, remoteEndPoint));
                    }
                    else
                    {
                        Core.GetService<DiscoveryService>().Discover(((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString());
                        client.Close();
                    }
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
                            Ip = remoteEndPoint.Address
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
                _ipDict.TryRemove(remoteEndPoint.Address, out _);
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

        private async Task<bool> ConnectToPeer(IPAddress ipAddress)
        {
            try
            {
                var client = new TcpClient();
                await client.ConnectAsync(ipAddress, APPPORT_TCP);

                var remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint!;

                // Check if already connected
                if (_clients.ContainsKey(remoteEndPoint))
                {
                    client.Close();
                    return true;
                }

                _clients[remoteEndPoint] = client;
                _ipDict[remoteEndPoint.Address] = remoteEndPoint;

                _ = Task.Run(() => ReceiveLoop(client, remoteEndPoint));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [EventHandler]
        public async void OnTcpMsgSend(TcpMsgSendEvent e)
        {
            // Check if we have a connected client
            if (!_ipDict.TryGetValue(e.Ip, out var remoteEndPoint) || !_clients.TryGetValue(remoteEndPoint, out var client) || !client.Connected)
            {
                // Try to connect first
                var connected = await ConnectToPeer(e.Ip);
                if (!connected)
                    return; // Connection failed, cannot send

                remoteEndPoint = _ipDict[e.Ip];

                // Get the newly connected client
                if (!_clients.TryGetValue(remoteEndPoint, out client))
                    return; // Still not available
            }

            try
            {
                var bytes = EncryptionService.Encode(e.Data);
                var lengthBytes = BitConverter.GetBytes(bytes.Length);
                var stream = client.GetStream();
                await stream.WriteAsync(lengthBytes);
                await stream.WriteAsync(bytes);
            }
            catch (Exception)
            {
                // Send failed - remove the client
                _clients.TryRemove(remoteEndPoint, out _);
                _ipDict.TryRemove(remoteEndPoint.Address, out _);
                client?.Dispose();
            }
        }
    }
}
