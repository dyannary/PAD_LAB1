using System.Text;
using System.Xml.Serialization;
using Resources;

namespace Subscriber
{
    public class ContentHandler
    {
        public static void Handle(byte[] contentBytes)
        {
            var contentString = Encoding.UTF8.GetString(contentBytes);
            var xmlSerializer = new XmlSerializer(typeof(Content));

            using (var reader = new StringReader(contentString))
            {
                var content = (Content)xmlSerializer.Deserialize(reader);

                Console.WriteLine(content.Message);
            }
        }
    }
}