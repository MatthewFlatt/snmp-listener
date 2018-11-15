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
    public class Worker
    {
        private readonly BlockingCollection<AlertData> _workerQueue;
        private readonly BlockingCollection<ActionData> _actionQueue;
        private AlertData _currentAlert;
        private readonly RulesList _rules;
        private static readonly ILog Logger = LogManager.GetLogger("Worker");

        public Worker(BlockingCollection<AlertData> queue, BlockingCollection<ActionData> actionQueue, RulesList rules)
        {
            _workerQueue = queue;
            _actionQueue = actionQueue;
            _rules = rules;
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    _currentAlert = _workerQueue.Take();
                    var matchingRules = _rules.GetMatchingRules(_currentAlert);
                    

                    foreach (var matchingRule in matchingRules)
                    {
                        _actionQueue.Add(new ActionData {   AlertForAction = _currentAlert,
                                                            ScriptToRun = matchingRule.PowerShellScriptFile,
                                                            SqlServerConnectionString = CreateSqlServerConnectionString(),
                                                            MachineAlert = GetMachineAlert(),
                                                            AdditionalObject = GetAdditionalObjects()
                        });
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("An exception occurred and the alert was dropped");
                    Logger.Error(e);
                }

            }
        }

        public string CreateSqlServerConnectionString()
        {
            if (GetMachineAlert())
            {
                return "";
            }

            if (_currentAlert.TargetObject == null)
            {
                return "";
            }
            var parts = _currentAlert.TargetObject.Split('>');
            if (parts[0].ToLower().Contains("(local)"))
            {
                return parts[0].Split('\\').First();
            }

            if (parts.Length == 1)
            {
                return parts[0];
            }
            
            return parts[1].Contains("(local)") ? parts[1].Split('\\').First() : parts[0].TrimEnd(' ');
        }

        public bool GetMachineAlert()
        {
            return Alerts.MachineAlerts.Contains(_currentAlert.AlertType);
        }

        public List<string> GetAdditionalObjects()
        {
            var objects = new List<string>();
            if (_currentAlert.TargetObject == null)
            {
                return objects;
            }
            foreach (var part in _currentAlert.TargetObject.Split('>').Skip(1))
            {
                if (GetMachineAlert() || !part.Contains('\\'))
                {
                    objects.Add(part);
                }
            }
            return objects;
        }
    }
}
