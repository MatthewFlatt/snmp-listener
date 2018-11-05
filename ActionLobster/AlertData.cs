using System;
using System.Linq;
using SnmpSharpNet;

namespace ActionLobster
{
    class AlertData
    {
        public int AlertId { get; }
        public string AlertType { get; }
        public string AlertDescription { get; }
        public DateTime EventTime { get; }
        public Severity CurrentSeverity { get; }
        public string TargetObject { get; }
        public string DetailsUrl { get; }
        public Status StatusChangeType { get; }
        public Severity PreviousWorstSeverity { get; }
        public string MachineName { get; }
        public string ClusterName { get; }
        public string GroupName { get; }

        public AlertData(Pdu snmpData)
        {
            foreach (var value in snmpData.VbList)
            {
                var id = GetId(value.Oid.ToString());
                switch (id)
                {
                    case 1:
                        AlertId = Int32.Parse(value.Value.ToString());
                        break;
                    case 2:
                        AlertType = value.Value.ToString();
                        break;
                    case 3:
                        AlertDescription = value.Value.ToString();
                        break;
                    case 4:
                        EventTime = DateTime.Parse(value.Value.ToString());
                        break;
                    case 5:
                        CurrentSeverity = StringToSeverity(value.Value.ToString());
                        break;
                    case 6:
                        TargetObject = value.Value.ToString();
                        break;
                    case 7:
                        DetailsUrl = value.Value.ToString();
                        break;
                    case 8:
                        StatusChangeType = StringToStatus(value.Value.ToString());
                        break;
                    case 9:
                        PreviousWorstSeverity = StringToSeverity(value.Value.ToString());
                        break;
                    case 10:
                        MachineName = value.Value.ToString();
                        break;
                    case 11:
                        ClusterName = value.Value.ToString();
                        break;
                    case 12:
                        GroupName = value.Value.ToString();
                        break;
                }
                
            }
        }

        private int GetId(string oid)
        {
            var parts = oid.Split('.');
            return Int32.Parse(parts.Last());
        }

        public override string ToString()
        {
            return
                $"AlertId : {AlertId}{Environment.NewLine}AlertType : {AlertType}{Environment.NewLine}AlertDescription : {AlertDescription}{Environment.NewLine}EventTime : {EventTime}{Environment.NewLine}CurrentServerity : {CurrentSeverity}{Environment.NewLine}TargetObject : {TargetObject}{Environment.NewLine}DetailsUrl : {DetailsUrl}{Environment.NewLine}StatusChangeType{StatusChangeType}{Environment.NewLine}PreviousWorstSeverity : {PreviousWorstSeverity}{Environment.NewLine}MachineName : {MachineName}{Environment.NewLine}ClusterName : {ClusterName}{Environment.NewLine}GroupName : {GroupName}";
        }

        public Severity StringToSeverity(string severity)
        {
            switch (severity)
            {
                case "Unknown":
                    return Severity.Unknown;
                case "None":
                    return Severity.None;
                case "Low":
                    return Severity.Low;
                case "Medium":
                    return Severity.Medium;
                case "High":
                    return Severity.High;
                default:
                    return Severity.Unknown;
            }
        }

        public Status StringToStatus(string status)
        {
            switch (status)
            {
                case "Raised":
                    return Status.Raised;
                case "Ended":
                    return Status.Ended;
                case "Escalated":
                    return Status.Escalated;
                default:
                    return Status.Unknown;
            }
        }
    }
}
