using System;
using System.Collections.Generic;
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

        public void UpdateRules(string json)
        {
            try
            {
                Rules = JsonConvert.DeserializeObject<List<Rule>>(json);
            }
            catch (Exception e)
            {
                Logger.Error("Error updating rules");
                Logger.Error(e);
            }
            
        }

        public List<Rule> GetMatchingRules(AlertData data)
        {
            var matchingRules = new List<Rule>();
            foreach (var rule in Rules)
            {
                if (rule.RuleMatches(data.AlertType, data.ClusterName,
                    data.GroupName,
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
