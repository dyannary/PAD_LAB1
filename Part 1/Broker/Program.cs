﻿using Broker;

Console.WriteLine("Broker");

BrokerSocket brokerSocket = new BrokerSocket("192.168.1.128", 9000);

brokerSocket.Start(10);

//long running tasks, and no long running
var worker = new MessageWorker();
Task.Factory.StartNew(worker.DoSendMessageWork, TaskCreationOptions.LongRunning);

Console.ReadLine();