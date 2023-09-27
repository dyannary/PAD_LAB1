using System.Net.Sockets;

namespace Resources
{
    public class ConnectionInfo
    {
        public const int BuffSize = 1024;

        public byte[] Data { get; set; }

        public Socket Socket { get; set; }

        public string Address { get; set; }

        public string Topic { get; set; }

        public ConnectionInfo()
        {
            Data = new byte[BuffSize];
        }

    }
}
