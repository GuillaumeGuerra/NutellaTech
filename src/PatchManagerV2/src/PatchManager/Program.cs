using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace PatchManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseWebListener()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration() // I don't even know this is supposed to do :)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
