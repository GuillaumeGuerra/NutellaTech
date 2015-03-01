using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(NotificationService)))
            {
                Console.WriteLine("Loading Db context");
                OneNoteDb.Instance.LoadContext();
                Console.WriteLine("Db context loaded");

                Console.WriteLine("Starting service");
                host.Open();
                Console.WriteLine("The service is ready"); 

                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                Console.WriteLine("Stopping service");
                host.Close();
                Console.WriteLine("Service stopped");

                Console.WriteLine("Disposing Db context");
                OneNoteDb.Instance.Dispose();
                Console.WriteLine("Db context disposed");
            }
        }
    }
}
