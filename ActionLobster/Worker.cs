using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActionLobster
{
    class Worker
    {
        private BlockingCollection<AlertData> WorkerQueue;

        public Worker(BlockingCollection<AlertData> queue)
        {
            WorkerQueue = queue;
        }

        public void Start()
        {
            while (true)
            {
                var alert = WorkerQueue.Take();
                Console.WriteLine("Taken data from queue");
                Console.WriteLine(alert.AlertType == "Database unavailable"
                    ? "DATABASE UNAVAILABLE ALERT"
                    : alert.ToString());
            }
        }
    }
}
