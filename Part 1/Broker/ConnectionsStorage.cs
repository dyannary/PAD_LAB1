using Resources;

namespace Broker
{
    static class ConnectionsStorage
    {
        private static readonly List<ConnectionInfo> Connections;
        private static readonly object Locker;

        static ConnectionsStorage()
        {
            Connections = new List<ConnectionInfo>();
            Locker = new object();
        }

        public static void Add(ConnectionInfo connectionInfo)
        {
            lock (Locker)
            {
                Connections.Add(connectionInfo);
            }
        }

        public static void Remove(string address)
        {
            lock (Locker)
            {
                Connections.RemoveAll(x => x.Address == address);
            }
        }

        public static List<ConnectionInfo> GetConnectionInfos(string topic)
        {
            List<ConnectionInfo> selectedConnectionInfos;
            lock (Locker)
            {
                selectedConnectionInfos = Connections.Where(x => x.Topic == topic).ToList();
            }

            return selectedConnectionInfos;
        }
    }
}
