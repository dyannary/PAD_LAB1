using Broker;

Console.WriteLine("Broker");

BrokerSocket brokerSocket = new BrokerSocket("192.168.1.104", 5005);

brokerSocket.Start(10);

var worker = new MessageWorker();
Task.Factory.StartNew(worker.DoSendMessageWork, TaskCreationOptions.LongRunning);
 
Console.ReadLine();