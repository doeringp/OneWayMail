using System;
using System.ServiceProcess;

namespace Doering.OneWayMail.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var service = new Service1();

            if (Environment.UserInteractive)
            {
                try
                {
                    service.Start();
                    Console.WriteLine("Press any key to stop the service...");
                    Console.ReadKey();
                }
                finally
                {
                    service.Stop();
                }
            }
            else
            {
                ServiceBase.Run(service);
            }
        }
    }
}
