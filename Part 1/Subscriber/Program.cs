using Subscriber;

Console.WriteLine("Subscriber");

string topic;

Console.Write("Enter the topic: ");
topic = Console.ReadLine().ToLower();

var subscriberSocket = new SubscriberSocket(topic);

subscriberSocket.Connect("192.168.1.128", 9000);

Console.WriteLine("Press any key to exit");

Console.ReadLine();