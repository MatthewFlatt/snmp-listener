using SnmpSharpNet;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;

namespace ActionLobster
{
    class Program
    {
        static void Main(string[] args)
        {
            //var config = new Configuration();

            //
            // Show Infos
            //
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine("ActionLobster V0.0.{0}", version.Build);
            Console.WriteLine("-------------------------");
            
            // Create Queue
            var alertQueue = new BlockingCollection<AlertData>();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                var worker = new Worker(alertQueue);
                worker.Start();
            }).Start();
            
            //
            // Start listener
            //
            var listener = new SnmpListener();
            while(true) {
                SnmpData data = listener.Listen();
                if (data != null) {
                    Console.WriteLine("");
                    Console.WriteLine("Received package from: {0}", data.Sender);
                    Console.WriteLine("Adding to queue");
                    alertQueue.Add(data.AlertData);
                }
            }
        }
    }
}
