using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ActionLobster
{
    class RulesList
    {
        public List<Rule> Rules { get; private set; }

        public void UpdateRules(string json)
        {
            var jsonObject = JsonConvert.DeserializeObject(json);
            Rules = JsonConvert.DeserializeObject<List<Rule>>(json);
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
