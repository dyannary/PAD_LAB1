using Resources;

namespace Broker
{
    static class ConnectionsStorage
    {
        private static List<ConnectionInfo> _connections;
        private static object _locker;

        static ConnectionsStorage()
        {
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
