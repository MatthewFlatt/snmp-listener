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
        private BlockingCollection<AlertData> ActionQueue;

        public Worker(BlockingCollection<AlertData> queue, BlockingCollection<AlertData> actionQueue)
        {
            WorkerQueue = queue;
            ActionQueue = actionQueue;
        }

        public void Start()
        {
            while (true)
            {
                var alert = WorkerQueue.Take();
                Console.WriteLine("WORKER : Taken data from queue");
                if (alert.AlertType == "Database unavailable")
                {
                    Console.WriteLine("WORKER : DATABASE UNAVAILABLE ALERT");
                }
                else
                {
                    ActionQueue.Add(alert);
                }       
            }
        }
    }
}
