using System;
using NUnit.Framework;
using SnmpSharpNet;
using AlertActioner;

namespace AlertActioner.Tests
{
    [TestFixture]
    public class AlertDataTests
    {
        [TestCase("Ended", Status.Ended)]
        [TestCase("Escalated", Status.Escalated)]
        [TestCase("Raised", Status.Raised)]
        [TestCase("Unknown", Status.Unknown)]
        [TestCase("blah", Status.Unknown)]
        public void StringToStatusTest(string status, Status desiredStatus)
        {
            var alertData = new AlertData(new Pdu());
            var result = alertData.StringToStatus(status);
            Assert.AreEqual(desiredStatus, result);
        }

        [TestCase("High", Severity.High)]
        [TestCase("Medium", Severity.Medium)]
        [TestCase("Low", Severity.Low)]
        [TestCase("None", Severity.None)]
        [TestCase("Unknown", Severity.Unknown)]
        [TestCase("blah", Severity.Unknown)]
        public void StringToSeverityTest(string severity, Severity desiredSeverity)
        {
            var alertData = new AlertData(new Pdu());
            var result = alertData.StringToSeverity(severity);
            Assert.AreEqual(desiredSeverity, result);
        }
    }
}
