using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine($"Wait until {20 - i} seconds to suicide");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Good Bye !");
            Thread.Sleep(1000);
        }
    }
}
