using System.Threading.Channels;
using Subscriber;

Console.WriteLine("Subscriber");

string topic = "";

while (string.IsNullOrWhiteSpace(topic))
{
    Console.Write("Enter the topic: ");
    topic = Console.ReadLine().ToLower();

    if (string.IsNullOrWhiteSpace(topic))
    {
        Console.WriteLine("Please enter a valid topic");
    }
}

var subscriberSocket = new SubscriberSocket(topic);

try
{
    subscriberSocket.Connect("127.0.0.1", 9000);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

Console.WriteLine("Press any key to exit");

Console.ReadLine();