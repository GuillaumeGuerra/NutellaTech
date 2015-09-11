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
            Task.Factory.StartNew(() => LogAliveInfiniteLoop());
            Task.Factory.StartNew(() => NewThreadsLoop());

            Parallel.ForEach(Enumerable.Range(1, 10), i => Wait1(i));
            Console.ReadLine();
        }

        private static void NewThreadsLoop()
        {
            while (true)
            {
                Task.Factory.StartNew(() => ShortLifeThread(1));
                Task.Factory.StartNew(() => ShortLifeThread(2));

                Thread.Sleep(5000);
            }
        }

        private static void ShortLifeThread(int i)
        {
            Thread.CurrentThread.Name = string.Format("ShortLifeThread - {0}", i);
            Thread.Sleep(5000);
        }

        private static void LogAliveInfiniteLoop()
        {
            while (true)
            {
                Console.WriteLine("{0} => I am alive !", DateTime.Now.ToString("HH:mm:ss"));
                Thread.Sleep(1000);
            }
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
