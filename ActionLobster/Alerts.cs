using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionLobster
{
    class Alerts
    {
        public static List<string> MachineAlerts =  new List<string> {"Processor utilization", "Cluster failover", "Processor under-utilization", "Machine unreachable", "Low disk space", "Physical memory", "Clock skew", "Browser Service status", "VSS Writer status", "Monitoring stopped (host)", "Monitoring error (host)" };
        public static List<string> SqlServerAlerts = new List<string> { "Error log entry" };
        public static List<string> AvailabilityGroupAlerts = new List<string> { "AG failover" };
        public static List<string> AzureSqlDatabaseAlerts = new List<string>{ "CPU utilization" };
    }
}
