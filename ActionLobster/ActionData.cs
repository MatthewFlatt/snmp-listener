using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionLobster
{
    class ActionData
    {
        public AlertData AlertForAction { get; }
        public string ScriptToRun { get; }
        public string SqlServerConnectionString { get; }
        public bool MachineAlert { get; }
        // Database name, job name, disk letter etc
        public List<string> AdditionalObject { get; }

        public ActionData(AlertData alert, string script, string sqlServer, bool machineAlert, List<string> additionalObject)
        {
            AlertForAction = alert;
            ScriptToRun = script;
            SqlServerConnectionString = sqlServer;
            MachineAlert = machineAlert;
            AdditionalObject = additionalObject;
        }
    }
}
