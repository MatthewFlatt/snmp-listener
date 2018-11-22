using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace AlertActioner
{
    class ComponentStartup
    {
        private static readonly ILog Logger = LogManager.GetLogger("Component startup");
        public void Start()
        {
            // Create Queues
            var alertQueue = new BlockingCollection<AlertData>();
            var actionQueue = new BlockingCollection<ActionData>();
            

            var rules = new RulesList();
            rules.UpdateRules(ConfigurationHandler.GetRulesFileLocation());

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
            while (true)
            {
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
