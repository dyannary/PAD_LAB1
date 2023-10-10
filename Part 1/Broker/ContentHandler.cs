using System.Text;
using System.Xml.Serialization;
using Resources;

namespace Broker
{
    public class ContentHandler
    {
        public static void Handle(byte[] contentBytes, ConnectionInfo connectionInfo)
        {
            var contentString = Encoding.UTF8.GetString(contentBytes);

            if (contentString.StartsWith("subscribe#"))
            {
                connectionInfo.Topic = contentString.Split("subscribe#").LastOrDefault();
                ConnectionsStorage.Add(connectionInfo);
            }
            else if (contentString.StartsWith("unsubscribe#"))
            {
                string topic = contentString.Split("unsubscribe#").LastOrDefault();
                ConnectionsStorage.RemoveConnection(connectionInfo.Address, topic);
                Console.WriteLine($"{connectionInfo.Address} is removed");
            }
            else if (contentString.StartsWith("topics#"))
            {
                connectionInfo.Socket.Send(
                    Encoding.UTF8.GetBytes(String.Join("\n", ConnectionsStorage.Topics.ToArray())));
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Content));

                using TextReader reader = new StringReader(contentString);
                Content content = (Content)serializer.Deserialize(reader);
                ConnectionsStorage.AddTopic(content.Topic);
                ContentStorage.AddContent(content);
            }

            Console.WriteLine(contentString);
        }
    }
}