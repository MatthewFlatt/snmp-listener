using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace AlertActioner
{
    public partial class AlertActionerService : ServiceBase
    {
        private static readonly ILog Logger = LogManager.GetLogger("Main");

        public AlertActionerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Logger.Info($"AlertActioner V0.0.{version.Build}");
            Logger.Info($"Starting at {DateTime.Now}");
            Logger.Info("-------------------------");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                var componentStartup = new ComponentStartup();
                componentStartup.Start();
            }).Start();
        }

        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.ReadLine();
            OnStop();
        }

        protected override void OnStop()
        {
            Logger.Info("Shutting down");
        }

    }
}
