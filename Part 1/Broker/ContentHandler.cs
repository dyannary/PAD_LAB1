﻿using System.Text;
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
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Content));

                using TextReader reader = new StringReader(contentString);
                Content content = (Content)serializer.Deserialize(reader);
                ContentStorage.AddContent(content);
            }

            Console.WriteLine(contentString);
        }
    }
}
