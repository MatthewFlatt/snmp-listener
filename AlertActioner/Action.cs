using System;
using System.Collections.Concurrent;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace AlertActioner
{
    class Action
    {
        private readonly BlockingCollection<ActionData> _actionQueue;
        private static readonly ILog Logger = LogManager.GetLogger("Action");

        public Action(BlockingCollection<ActionData> actionQueue)
        {
            _actionQueue = actionQueue;
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    var action = _actionQueue.Take();
                    Logger.Debug("ACTION : Taken data from queue");
                    Logger.Debug($"ACTION : {action.AlertForAction}");
                    Logger.Debug($"ACTION : SQL Server connection string - {action.SqlServerConnectionString}");
                    Logger.Debug($"ACTION : Machine alert - {action.MachineAlert}");
                    foreach (var property in action.AdditionalObject)
                    {
                        Logger.Debug($"ACTION : Property : {property}");
                    }

                    using (PowerShell shell = PowerShell.Create())
                    {
                        var sb = new StringBuilder();
                        shell.Commands.AddScript("Set-ExecutionPolicy -ExecutionPolicy ByPass -Scope Process -Force");
                        sb.Append($"\" {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, action.ScriptToRun)}\"");
                        sb.Append($" -AlertId \"{action.AlertForAction.AlertId}\"");
                        sb.Append($" -AlertType \"{action.AlertForAction.AlertType}\"");
                        sb.Append($" -AlertDescription \"{action.AlertForAction.AlertDescription}\"");
                        sb.Append($" -EventTime \"{action.AlertForAction.EventTime}\"");
                        sb.Append($" -CurrentSeverity \"{action.AlertForAction.CurrentSeverity}\"");
                        sb.Append($" -TargetObject \"{action.AlertForAction.TargetObject}\"");
                        sb.Append($" -DetailsUrl \"{action.AlertForAction.DetailsUrl}\"");
                        sb.Append($" -StatusChangeType \"{action.AlertForAction.StatusChangeType}\"");
                        sb.Append($" -PreviousWorstSeverity \"{action.AlertForAction.PreviousWorstSeverity}\"");
                        sb.Append($" -MachineName \"{action.AlertForAction.MachineName}\"");
                        sb.Append($" -ClusterName \"{action.AlertForAction.ClusterName}\"");
                        sb.Append($" -GroupName \"{action.AlertForAction.GroupNamesToSingleString()}\"");
                        sb.Append($" -SqlServerConnectionString \"{action.SqlServerConnectionString}\"");

                        if (action.AdditionalObject.Count > 0)
                        {
                            sb.Append($" -ObjectName \"{action.AdditionalObject.First()}\"");
                        }
                        sb.Append(" -Verbose");

                        shell.AddScript(sb.ToString());
                        shell.Invoke();
                        foreach (var result in shell.Streams.Verbose)
                        {
                            Logger.Info($"POWERSHELL : {result}");
                        }

                        foreach (var errorRecord in shell.Streams.Error)
                        {
                            Logger.Error(errorRecord);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Error occured while attempting to run script");
                    Logger.Error(e);
                }
            }
        }
    }
}
