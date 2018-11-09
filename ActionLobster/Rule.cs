using System;
using System.Collections.Generic;

namespace ActionLobster
{
    class Rule
    {
        public List<string> AlertType { get; set; }
        public DateTime ActionFrom { get; set; }
        public DateTime ActionTo { get; set; }
        public List<string> IncludedServers { get; set; }
        public List<string> IncludedGroups { get; set; }
        public string PowerShellScriptFile { get; set; }
        public Severity MinimumSeverity { get; set; }
        public int Priority { get; set; }

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
