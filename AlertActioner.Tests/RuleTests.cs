using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AlertActioner.Tests
{
    [TestFixture]
    public class RuleTests
    {
        [Test]
        public void AlertTypeMatchTests()
        {
            var ruleToTest = new Rule {AlertType = new List<string>()};
            // No rule specified should return true
            Assert.IsTrue(ruleToTest.AlertTypeMatches("Database unavailable"));

            // Matches types in rule
            ruleToTest.AlertType = new List<string> {"Database unavailable", "Job failed"};
            Assert.IsTrue(ruleToTest.AlertTypeMatches("Database unavailable"));
            Assert.IsTrue(ruleToTest.AlertTypeMatches("Job failed"));

            // Does not match
            Assert.IsFalse(ruleToTest.AlertTypeMatches("Job duration unusual"));
        }

        [Test]
        public void ServerNameMatchTests()
        {
            var ruleToTest = new Rule { IncludedServers = new List<string>() };
            // No rule specified should return true
            Assert.IsTrue(ruleToTest.ServerNameMatches("server1"));

            // Matches servers in rule
            ruleToTest.IncludedServers = new List<string> { "server1", "server3" };
            Assert.IsTrue(ruleToTest.ServerNameMatches("server1"));
            Assert.IsTrue(ruleToTest.ServerNameMatches("server3"));

            // Does not match
            Assert.IsFalse(ruleToTest.ServerNameMatches("server2"));
        }

        [Test]
        public void GroupNameMatchTests()
        {
            var ruleToTest = new Rule { IncludedGroups = new List<string>() };
            // No rule specified should return true
            Assert.IsTrue(ruleToTest.GroupNameMatches(new List<string> {"group1"}));

            // 1 group matches in rule
            ruleToTest.IncludedGroups = new List<string> { "group1", "group3" };
            Assert.IsTrue(ruleToTest.GroupNameMatches(new List<string> { "group1" }));
            Assert.IsTrue(ruleToTest.GroupNameMatches(new List<string> { "group3" }));
            Assert.IsTrue(ruleToTest.GroupNameMatches(new List<string> { "group2","group3" }));

            // Does not match
            Assert.IsFalse(ruleToTest.GroupNameMatches(new List<string> { "group2" }));
        }

        [Test]
        public void TimeRangeMatchTests()
        {
            var ruleToTest = new Rule
            {
                ActionFrom = DateTime.MinValue,
                ActionTo = DateTime.MinValue
            };
            // Times match so return true
            Assert.IsTrue(ruleToTest.InTimeRange(DateTime.Now));

            // In time range (dates ignored)
            ruleToTest.ActionFrom = new DateTime(2018, 11, 1, 9, 0, 0);
            ruleToTest.ActionTo = new DateTime(2019, 11, 1, 17, 0, 0);
            Assert.IsTrue(ruleToTest.InTimeRange(new DateTime(2020, 11, 1, 12, 0, 0)));

            // Does not match
            Assert.IsFalse(ruleToTest.InTimeRange(new DateTime(2020, 11, 1, 8, 0, 0)));
            Assert.IsFalse(ruleToTest.InTimeRange(new DateTime(2020, 11, 1, 18, 0, 0)));
        }

        [Test]
        public void SeverityMatchTests()
        {
            var ruleToTest = new Rule { MinimumSeverity = Severity.Unknown};
            // No rule specified should return true
            Assert.IsTrue(ruleToTest.SeverityMatches(Severity.Unknown));

            // Matches types in rule
            ruleToTest.MinimumSeverity = Severity.Medium;
            Assert.IsTrue(ruleToTest.SeverityMatches(Severity.Medium));
            Assert.IsTrue(ruleToTest.SeverityMatches(Severity.High));

            // Does not match
            Assert.IsFalse(ruleToTest.SeverityMatches(Severity.Low));
        }

    }
}
