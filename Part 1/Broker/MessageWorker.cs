using System.Xml.Serialization;
using Resources;

namespace Broker
{
    public class MessageWorker
    {
        public void DoSendMessageWork()
        {
            while (true)
            {
                while (!ContentStorage.IsEmpty())
                {
                    try
                    {
                        var content = ContentStorage.GetNext();
                        if (content != null)
                        {
                            var connections = ConnectionsStorage.GetConnectionInfos(content.Topic);

                            foreach (var connection in connections)
                            {
                                var serializer = new XmlSerializer(typeof(Content));

                                using var memoryStream = new MemoryStream();
                                serializer.Serialize(memoryStream, content);
                                var data = memoryStream.ToArray();
                                connection.Socket.Send(data);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error in sending data: " + e);
                        throw;
                    }
                }
                Thread.Sleep(500);
            }
        }
    }
}
