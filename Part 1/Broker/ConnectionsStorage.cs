using System.Net.Sockets;
using Resources;

namespace Broker
{
    static class ConnectionsStorage
    {
        public static List<string> Topics;
        private static List<ConnectionInfo> _connections;
        private static object _locker;

        static ConnectionsStorage()
        {
            Topics = new List<string>();
            _connections = new List<ConnectionInfo>();
            _locker = new object();
        }

        public static void Add(ConnectionInfo connectionInfo)
        {
            lock (_locker)
            {
                _connections.Add(connectionInfo);
            }
        }

        public static void AddTopic(string topic)
        {
            if (!Topics.Contains(topic))
            {
                Topics.Add(topic);
            }
        }

        public static void RemoveConnection(string address, string topic)
        {
            lock (_locker)
            {
                _connections.RemoveAll(x => x.Address == address && x.Topic == topic);
            }
        }
        public static void Remove(string address)
        {
            lock (_locker)
            {
                _connections.RemoveAll(x => x.Address == address);
            }
        }
        public static List<ConnectionInfo> GetConnectionInfos(string topic)
        {
            List<ConnectionInfo> selectedConnectionInfos;
            lock (_locker)
            {
                selectedConnectionInfos = _connections.Where(x => x.Topic == topic).ToList();
            }

            return selectedConnectionInfos;
        }
    }
}