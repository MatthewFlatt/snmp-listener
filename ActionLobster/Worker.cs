using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActionLobster
{
    class Worker
    {
        private readonly BlockingCollection<AlertData> _workerQueue;
        private readonly BlockingCollection<ActionData> _actionQueue;
        private AlertData _currentAlert;

        public Worker(BlockingCollection<AlertData> queue, BlockingCollection<ActionData> actionQueue)
        {
            _workerQueue = queue;
            _actionQueue = actionQueue;
        }

        public void Start()
        {
            var firstRule = new Rule(new List<string>(), DateTime.Today, DateTime.Today, new List<string>(), new List<string>(), "ExampleScript.ps1");
            while (true)
            {
                _currentAlert = _workerQueue.Take();
                Console.WriteLine("WORKER : Taken data from queue");
                if (firstRule.RuleMatches(_currentAlert.AlertType, _currentAlert.ClusterName, _currentAlert.GroupName,
                    _currentAlert.EventTime))
                {
                    var action = new ActionData(_currentAlert, firstRule.PowerShellScriptFile, CreateSqlServerConnectionString(), GetMachineAlert(), GetAdditionalObjects());
                    _actionQueue.Add(action);
                }
                else
                {
                    Console.WriteLine("WORKER : ALERT DID NOT MATCH RULE");
                }
                
    
            }
        }

        private string CreateSqlServerConnectionString()
        {
            if (GetMachineAlert())
            {
                return "";
            }
            var parts = _currentAlert.TargetObject.Split('>');
            if (parts[0].ToLower().Contains("(local"))
            {
                return parts[0].Split('\\').First();
            }

            if (parts.Length == 1)
            {
                return parts[0];
            }
            
            return parts[1].Contains("(local)") ? parts[1].Split('\\').First() : parts[0].TrimEnd(' ');
        }

        private bool GetMachineAlert()
        {
            return Alerts.MachineAlerts.Contains(_currentAlert.AlertType);
        }

        private List<string> GetAdditionalObjects()
        {
            var objects = new List<string>();
            foreach (var part in _currentAlert.TargetObject.Split('>').Skip(1))
            {
                if (string.IsNullOrWhiteSpace(_currentAlert.MachineName))
                {
                    objects.Add(part);
                }
            }
            return objects;
        }
    }
}
