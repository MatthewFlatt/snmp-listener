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
                var query = $"";
            }
            return alerts;
        }
    }
}
