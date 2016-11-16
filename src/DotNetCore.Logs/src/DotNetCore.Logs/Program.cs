using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Logs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loggerFactory = new LoggerFactory()
               .AddConsole()
               .AddDebug();
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation(
              "This is a test of the emergency broadcast system.");

            Console.Read();
        }
    }
}
