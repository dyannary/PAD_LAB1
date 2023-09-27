using System.Net;
using System.Net.Sockets;
using System.Text;
using Resources;

namespace Subscriber
{
    public class SubscriberSocket
    {
        private Socket _socket;

        private string _topic;

        public SubscriberSocket(string topic)
        {
            _topic = topic;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallback, null);
            Console.WriteLine("Waiting for a connection");
        }

        private void ConnectedCallback(IAsyncResult asyncResult)
        {
            if (_socket.Connected)
            {
                Console.WriteLine("Subscriber connected to broker.");
                Subscribe();
                StartReceive();
            }
            else
            {
                Console.WriteLine("Error: Subscriber could not connected to broker.");
            }
        }

        private void Subscribe()
        {
            //adaug patternul in setari, ca sa fie mai decuplat
            var data = Encoding.UTF8.GetBytes("subscribe#" + _topic);
            Send(data);
        }

        private void Send(byte[] data)
        {
            try
            {
                _socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not send data:" + e.Message);
            }
        }

        private void StartReceive()
        {
            ConnectionInfo connectionInfo = new ConnectionInfo();
            connectionInfo.Socket = _socket;

            _socket.BeginReceive(connectionInfo.Data, 0, connectionInfo.Data.Length,
                SocketFlags.None, ReceiveCallback, connectionInfo);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connectionInfo = asyncResult.AsyncState as ConnectionInfo;

            try
            {
                SocketError response;
                int buffsize = _socket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success)
                {
                    byte[] contentBytes = new byte[buffsize];

                    Array.Copy(connectionInfo.Data, contentBytes, contentBytes.Length);

                    //Handler
                    ContentHandler.Handle(contentBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error. Cannot receive data from broker." + e.Message);
            }
            finally
            {
                try
                {
                    connectionInfo.Socket.BeginReceive(connectionInfo.Data, 0, connectionInfo.Data.Length,
                        SocketFlags.None, ReceiveCallback, connectionInfo);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //se poate de schimbat
                    connectionInfo.Socket.Close();
                }
            }
        }
    }
}
