using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Data.SqlClient;

namespace AlertActioner
{
    class RepositoryPoller
    {
        private static readonly ILog Logger = LogManager.GetLogger("Repository poller");
        private string _connectionString;
        private long _lastPollTime;

        public RepositoryPoller(string connectionString)
        {
            _connectionString = connectionString;
            _lastPollTime = DateTime.UtcNow.Ticks;
        }

        public List<AlertData> Poll()
        {
            var alerts = new List<AlertData>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = $"SELECT a.AlertId , at.ShortName , at.Description , a.TargetObject , a.Raised , a.ClearedDate , a.WorstSeverity , a.LastSeverity FROM   alert.Alert AS a\r\n       JOIN alert.Alert_Type AS at ON at.AlertType = a.AlertType\r\nWHERE  a.ClearedDate > @lastPoll;";
            }
            return alerts;
        }
    }
}
