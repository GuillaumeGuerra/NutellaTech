using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        private static ManualResetEvent _handle = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("Current App PID {0}", Process.GetCurrentProcess().Id);

            Task.Factory.StartNew(() => InfiniteLoop1(1));
            Task.Factory.StartNew(() => InfiniteLoop1(2));

            Parallel.ForEach(Enumerable.Range(1, 10), i => Wait1(i));
            Console.ReadLine();
        }

        private static void InfiniteLoop1(int i)
        {
            Thread.CurrentThread.Name = string.Format("InfiniteLoopThread - {0}", i);

            InfiniteLoop2();
        }

        private static void InfiniteLoop2()
        {
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        private static void Wait1(int i)
        {
            Thread.CurrentThread.Name = string.Format("WaitThread - {0}", i);

            Wait2();
        }

        private static void Wait2()
        {
            Wait3();
        }

        private static void Wait3()
        {
            WaitOne();
        }

        private static void WaitOne()
        {
            _handle.WaitOne();
        }
    }
}
