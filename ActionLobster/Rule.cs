using System;
using System.Collections.Generic;

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
        private Severity MinimumSeverity { get; }
        public int Priority { get; }
    
    

        public Rule(List<string> alertType, DateTime actionFrom, DateTime actionTo, List<string> includedServers,
            List<string> includedGroups, Severity minimumSeverity, int priority, string powerShellScriptFile)
        {
            AlertType = alertType;
            ActionFrom = actionFrom;
            ActionTo = actionTo;
            IncludedServers = includedServers;
            IncludedGroups = includedGroups;
            PowerShellScriptFile = powerShellScriptFile;
            MinimumSeverity = minimumSeverity;
            Priority = priority;
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

        private bool SeverityMatches(Severity severity)
        {
            return severity >= MinimumSeverity;
        }

        public bool RuleMatches(string alertType, string serverName, string groupName, DateTime alertTime, Severity severity)
        {
            return AlertTypeMatches(alertType) && ServerNameMatches(serverName) && GroupNameMatches(groupName) &&
                   InTimeRange(alertTime) && SeverityMatches(severity);
        }

}
}
