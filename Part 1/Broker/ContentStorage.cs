using System.Collections.Concurrent;
using Resources;

namespace Broker
{
    static class ContentStorage
    {
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

        public static bool IsEmpty()
        {
            return _contentQueue.IsEmpty;
        }
    }
}
