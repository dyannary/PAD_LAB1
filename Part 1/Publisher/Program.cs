using System.Xml.Serialization;
using Publisher;
using Resources;

Console.WriteLine("Publisher");

var publisherSocket = new PublisherSocket();
publisherSocket.Connect("127.0.0.1", 9000);

if (publisherSocket.isConnected)
{
    while (true)
    {
        var content = new Content();

        Console.Write("Topic: ");
        content.Topic = Console.ReadLine().ToLower();

        Console.Write("Message: ");
        content.Message = Console.ReadLine();

        var prString = new XmlSerializer(typeof(Content));

        using var memoryStream = new MemoryStream();

        prString.Serialize(memoryStream, content);

        var data = memoryStream.ToArray();

        publisherSocket.Send(data);
    }
} 