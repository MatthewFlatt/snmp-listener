using SnmpSharpNet;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Core;

namespace AlertActioner
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger("Main");
        static void Main(string[] args)
        {

            // Show Infos
            //
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Logger.Info($"AlertActioner V0.0.{version.Build}");
            Logger.Info($"Starting at {DateTime.Now}");
            Logger.Info("-------------------------");
            
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
                Logger.Error("Error reading rules from file");
                Logger.Error(e);
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
                        Logger.Debug("Adding to queue");
                        alertQueue.Add(data.AlertData);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Error adding trap to queue");
                    Logger.Error(e);
                }
            }
        }
    }
}
