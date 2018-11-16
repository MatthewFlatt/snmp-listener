using System;
using System.Collections.Generic;

namespace AlertActioner
{
    public class Rule
    {
        public List<string> AlertType { get; set; }
        public DateTime ActionFrom { get; set; }
        public DateTime ActionTo { get; set; }
        public List<string> IncludedServers { get; set; }
        public List<string> IncludedGroups { get; set; }
        public string PowerShellScriptFile { get; set; }
        public Severity MinimumSeverity { get; set; }
        public int Priority { get; set; }

        public bool AlertTypeMatches(string alertType)
        {
            return AlertType == null || AlertType.Count == 0 || AlertType.Contains(alertType);
        }

        public bool ServerNameMatches(string serverName)
        {
            return IncludedServers == null || IncludedServers.Count == 0 || IncludedServers.Contains(serverName);
        }

        public bool GroupNameMatches(List<string> groupNames)
        {
            if (IncludedGroups == null || IncludedGroups.Count == 0)
            {
                return true;
            }
            foreach (var groupName in groupNames)
            {
                if (IncludedGroups.Contains(groupName))
                {
                    return true;
                }
            }
            return false;
        }

        public bool InTimeRange(DateTime alertTime)
        {
            return ActionFrom.Equals(ActionTo) || alertTime.TimeOfDay > ActionFrom.TimeOfDay && alertTime.TimeOfDay < ActionTo.TimeOfDay;

        }

        public bool SeverityMatches(Severity severity)
        {
            return severity >= MinimumSeverity;
        }

        public bool RuleMatches(string alertType, string serverName, List<string> groupNames, DateTime alertTime, Severity severity)
        {
            return AlertTypeMatches(alertType) && ServerNameMatches(serverName) && GroupNameMatches(groupNames) &&
                   InTimeRange(alertTime) && SeverityMatches(severity);
        }

}
}
