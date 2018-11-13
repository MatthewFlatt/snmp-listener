using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertActioner
{
    class ActionData
    {
        public AlertData AlertForAction { get; set; }
        public string ScriptToRun { get; set; }
        public string SqlServerConnectionString { get; set; }
        public bool MachineAlert { get; set; }
        // Database name, job name, disk letter etc
        public List<string> AdditionalObject { get; set; }
    }
}
