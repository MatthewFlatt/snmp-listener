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
                var action = _actionQueue.Take();
                Console.WriteLine("ACTION : Taken data from queue");
                Console.WriteLine("ACTION : {0}",action.AlertForAction);
                Console.WriteLine("ACTION : SQL Server connection string - {0}",action.SqlServerConnectionString);
                Console.WriteLine("ACTION : Machine alert - {0}", action.MachineAlert);
                foreach(var property in action.AdditionalObject)
                {
                    Console.WriteLine("ACTION : Property : {0}", property);
                }
                PowerShell shell = PowerShell.Create();
                shell.Commands.AddScript("Set-ExecutionPolicy -ExecutionPolicy ByPass -Scope Process");
                shell.Commands.AddScript(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, action.ScriptToRun));
                shell.Commands.AddParameter("SqlServerConnectionString", action.SqlServerConnectionString);
                
                if (action.AdditionalObject.Count > 0)
                {
                    shell.Commands.AddParameter("ObjectName", action.AdditionalObject.First());
                }
                var results = shell.Invoke();
                foreach (var result in results)
                {
                    Console.WriteLine("POWERSHELL : {0}",result.ToString());
                }
            }
        }
    }
}
