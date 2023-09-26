using Subscriber;

Console.WriteLine("Subscriber");

string topic;

Console.Write("Enter the topic: ");
topic = Console.ReadLine().ToLower();

var subscriberSocket = new SubscriberSocket(topic);

subscriberSocket.Connect("127.0.0.1", 9000);

Console.WriteLine("Press any key to exit");

Console.ReadLine();