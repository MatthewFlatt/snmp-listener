using SnmpSharpNet;
using System;
using System.Collections.Concurrent;
using System.IO;
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
            
            // Create Queues
            var alertQueue = new BlockingCollection<AlertData>();
            var actionQueue = new BlockingCollection<ActionData>();
            var jsonRules = "";
            try
            {
                jsonRules =
                    File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExampleRules.json"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading rules from file");
                Console.WriteLine(e);
            }
            
            var rules = new RulesList();
            rules.UpdateRules(jsonRules);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                var worker = new Worker(alertQueue, actionQueue, rules);
                worker.Start();
            }).Start();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                var action = new Action(actionQueue);
                action.Start();
            }).Start();

            //
            // Start listener
            //
            var listener = new SnmpListener();
            while(true) {
                try
                {
                    SnmpData data = listener.Listen();
                    if (data != null)
                    {
                        Console.WriteLine("Adding to queue");
                        alertQueue.Add(data.AlertData);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error adding trap to queue");
                    Console.WriteLine(e);
                }
            }
        }
    }
}
