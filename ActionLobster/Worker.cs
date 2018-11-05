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
        private ConcurrentQueue<AlertData> WorkerQueue;

        public Worker(ConcurrentQueue<AlertData> queue)
        {
            WorkerQueue = queue;
        }

        public void Start()
        {
            while (true)
            {
                if (!WorkerQueue.TryDequeue(out var alert))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    continue;
                }
                Console.WriteLine("Taken data from queue");
                if (alert.AlertType == "Database unavailable")
                {
                    Console.WriteLine("DATABASE UNAVAILABLE ALERT");
                }
                else
                {
                    Console.WriteLine(alert.ToString());
                }
                
            }
        }
    }
}
