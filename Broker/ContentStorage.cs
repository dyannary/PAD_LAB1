using System.Collections.Concurrent;
using Resources;

namespace Broker
{
    static class ContentStorage
    {
        // persistent - salvarea po disk si restaurearea brokerului (durable queues)
        // transient - de durata scurta
        // colectiile concurente sunt mai rapide decat mecanismul stanndard de blocare 

        private static ConcurrentQueue<Content> _contentQueue;

        static ContentStorage()
        {
            _contentQueue = new ConcurrentQueue<Content>();
        }

        public static void AddContent(Content content)
        {
            _contentQueue.Enqueue(content); 
        }

        public static Content GetNext()
        {
            Content content = null;

            _contentQueue.TryDequeue(out content);

            return content;
        }

        //poate fi si ca proprietate
        public static bool IsEmpty()
        {
            return _contentQueue.IsEmpty;
        }
    }
}
