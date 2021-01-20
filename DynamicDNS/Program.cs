using System.ServiceProcess;

namespace DynamicDNS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new DynamicDNSService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
