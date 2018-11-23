using System;
using System.Collections.Generic;
using System.Data;
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
                var query = $"SELECT a.AlertId , at.ShortName , at.Description , a.ClearedDate , a.LastSeverity, a.WorstSeverity , a.TargetObject FROM alert.Alert AS a JOIN alert.Alert_Type AS at ON at.AlertType = a.AlertType WHERE  a.ClearedDate > @lastPoll;";
                var command = new SqlCommand(query);
                var param = new SqlParameter { ParameterName= "lastPoll", SqlDbType = SqlDbType.BigInt, Value = _lastPollTime};
                command.Parameters.Add(param);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            alerts.Add(new AlertData(values));
                        }
                    }
                }
                catch (SqlException e)
                {
                    Logger.Error("Error querying repository database", e);
                }
            }
            return alerts;
        }
    }
}
