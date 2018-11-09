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

namespace ActionLobster
{
    class Action
    {
        private readonly BlockingCollection<ActionData> _actionQueue;

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
                    Console.WriteLine("ACTION : Taken data from queue");
                    Console.WriteLine("ACTION : {0}", action.AlertForAction);
                    Console.WriteLine("ACTION : SQL Server connection string - {0}", action.SqlServerConnectionString);
                    Console.WriteLine("ACTION : Machine alert - {0}", action.MachineAlert);
                    foreach (var property in action.AdditionalObject)
                    {
                        Console.WriteLine("ACTION : Property : {0}", property);
                    }

                    using (PowerShell shell = PowerShell.Create())
                    {
                        var sb = new StringBuilder();
                        shell.Commands.AddScript("Set-ExecutionPolicy -ExecutionPolicy ByPass -Scope Process -Force");
                        sb.Append(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, action.ScriptToRun));
                        sb.Append($" -SqlServerConnectionString \"{action.SqlServerConnectionString}\"");

                        if (action.AdditionalObject.Count > 0)
                        {
                            sb.Append($" -ObjectName \"{action.AdditionalObject.First()}\"");
                        }

                        shell.AddScript(sb.ToString());
                        shell.Invoke();
                        foreach (var result in shell.Streams.Information)
                        {
                            Console.WriteLine("POWERSHELL : {0}", result);
                        }

                        foreach (var errorRecord in shell.Streams.Error)
                        {
                            Console.WriteLine(errorRecord);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occured while attempting to run script");
                    Console.WriteLine(e);
                }
            }
        }
    }
}
