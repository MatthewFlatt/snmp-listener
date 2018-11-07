using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionLobster
{
    class Rule
    {
        private List<string> AlertType { get; }
        private DateTime ActionFrom { get; }
        private DateTime ActionTo { get; }
        private List<string> IncludedServers { get; }
        private List<string> IncludedGroups { get; }
        public string PowerShellScriptFile { get; }

        public Rule(List<string> alertType, DateTime actionFrom, DateTime actionTo, List<string> includedServers,
            List<string> includedGroups, string powerShellScriptFile)
        {
            AlertType = alertType;
            ActionFrom = actionFrom;
            ActionTo = actionTo;
            IncludedServers = includedServers;
            IncludedGroups = includedGroups;
            PowerShellScriptFile = powerShellScriptFile;
        }

        private bool AlertTypeMatches(string alertType)
        {
            return AlertType.Count == 0 || AlertType.Contains(alertType);
        }

        private bool ServerNameMatches(string serverName)
        {
            return IncludedServers.Count == 0 || IncludedServers.Contains(serverName);
        }

        private bool GroupNameMatches(string groupName)
        {
            return IncludedGroups.Count == 0 || IncludedGroups.Contains(groupName);
        }

        private bool InTimeRange(DateTime alertTime)
        {
            return alertTime.TimeOfDay > ActionFrom.TimeOfDay && alertTime.TimeOfDay < ActionTo.TimeOfDay;

        }

        public bool RuleMatches(string alertType, string serverName, string groupName, DateTime alertTime)
        {
            return AlertTypeMatches(alertType) && ServerNameMatches(serverName) && GroupNameMatches(groupName) &&
                   InTimeRange(alertTime);
        }

}
}
