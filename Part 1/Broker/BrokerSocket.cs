using System.Net;
using System.Net.Sockets;
using Resources;

namespace Broker
{
    public class BrokerSocket
    {
        private Socket _socket;
        private readonly IPEndPoint _serverEndpoint;

        public BrokerSocket(string ip, int port)
        {
            var ipAddress = IPAddress.Parse(ip);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _serverEndpoint = new IPEndPoint(ipAddress, port);
        }

        public void Start(int queueLimit)
        {
            try
            {
                _socket.Bind(_serverEndpoint);
                _socket.Listen(queueLimit);
                Console.WriteLine("Server listening on " + _serverEndpoint);
                _ = ClientAcceptAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error binding and Listening" + e.Message);
            }
        }

        public async Task ClientAcceptAsync()
        {
            try
            {
                while (true)
                {
                    var socket = await _socket.AcceptAsync();
                    _ = HandleClientAsync(socket);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot accept. " + e.Message);
            }
        }

        private async Task HandleClientAsync(Socket socket)
        {
            var connectionInfo = new ConnectionInfo();

            try
            {
                connectionInfo.Socket = socket;
                connectionInfo.Address = socket.RemoteEndPoint.ToString();

                while (true)
                {
                    var buffSize = await connectionInfo.Socket.ReceiveAsync(connectionInfo.Data);

                    if (buffSize <= 0)
                        break;

                    var content = new byte[buffSize];
                    Array.Copy(connectionInfo.Data, content, content.Length);
                    ContentHandler.Handle(content, connectionInfo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot receive data: " + e.Message);

                var address = connectionInfo.Socket.RemoteEndPoint.ToString();

                ConnectionsStorage.Remove(address);

                connectionInfo.Socket.Close();
            }
        }
    }
}

