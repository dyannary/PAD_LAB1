using System.Collections.Concurrent;
using Resources;

namespace Broker
{
    static class ContentStorage
    {
        private static readonly ConcurrentQueue<Content> ContentQueue;

        static ContentStorage()
        {
            ContentQueue = new ConcurrentQueue<Content>();
        }

        public static void AddContent(Content content)
        {
            ContentQueue.Enqueue(content); 
        }

        public static Content? GetNext()
        {
            ContentQueue.TryDequeue(out var content);

            return content;
        }

        public static bool IsEmpty()
        {
            return ContentQueue.IsEmpty;
        }
    }
}
