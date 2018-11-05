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
        public string CurrentSeverity { get; }
        public string TargetObject { get; }
        public string DetailsUrl { get; }
        public string StatusChangeType { get; }
        public string PreviousWorstSeverity { get; }
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
                        CurrentSeverity = value.Value.ToString();
                        break;
                    case 6:
                        TargetObject = value.Value.ToString();
                        break;
                    case 7:
                        DetailsUrl = value.Value.ToString();
                        break;
                    case 8:
                        StatusChangeType = value.Value.ToString();
                        break;
                    case 9:
                        PreviousWorstSeverity = value.Value.ToString();
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
    }
}
