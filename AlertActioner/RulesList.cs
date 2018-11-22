using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

namespace AlertActioner
{
    public class RulesList
    {
        public List<Rule> Rules { get; private set; }
        private static readonly ILog Logger = LogManager.GetLogger("Main");

        public void UpdateRules(List<string> ruleFiles)
        {
            var allRules = new List<Rule>();
            foreach (var ruleFileLocation in ruleFiles)
            {
                try
                {
                    var json = File.ReadAllText(ruleFileLocation);
                    var rules = JsonConvert.DeserializeObject<List<Rule>>(json);
                    allRules = allRules.Concat(rules).ToList();
                }
                catch (Exception e)
                {
                    Logger.Error($"Error updating rules from file {ruleFileLocation}");
                    Logger.Error(e);
                }
            }

            if (allRules.Count == 0)
            {
                Logger.Error("Unable to load any rules file, shutting down");
                throw new Exception("Unable to load any rules file, shutting down");
            }

            Rules = allRules;
        }

        public List<Rule> GetMatchingRules(AlertData data)
        {
            var matchingRules = new List<Rule>();
            foreach (var rule in Rules)
            {
                if (rule.RuleMatches(data.AlertType, data.ClusterName,
                    data.GroupNames,
                    data.EventTime, data.CurrentSeverity))
                {
                    if (matchingRules.Count == 0)
                    {
                        matchingRules.Add(rule);
                    }
                    else
                    {
                        if (matchingRules.First().Priority == rule.Priority)
                        {
                            matchingRules.Add(rule);
                        }
                        else
                        {
                            if (matchingRules.First().Priority < rule.Priority)
                            {
                                matchingRules.Clear();
                                matchingRules.Add(rule);
                            }
                        }
                    }
                }
            }

            return matchingRules;
        }
    }
}
