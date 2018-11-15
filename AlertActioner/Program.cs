using System;
using System.ServiceProcess;

namespace AlertActioner
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new AlertActionerService();
            if (Environment.UserInteractive)
            {
                service.RunAsConsole(args);
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                {
                    service
                };
                ServiceBase.Run(servicesToRun);
            }
                      
        }
    }
}
