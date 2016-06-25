using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Front
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                //.UseKestrel()
                //.UseWebListener()
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
