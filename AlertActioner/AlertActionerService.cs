using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;

namespace AlertActioner
{
    public partial class AlertActionerService : ServiceBase
    {
        private ComponentStartup _componentStartup;
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
            _componentStartup = new ComponentStartup();
            _componentStartup.Run();
        }

        public void RunAsConsole(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStop()
        {
            _componentStartup.Dispose();
        }

    }
}
