using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    public class ConsoleObserver : IObserver
    {
        public StringBuilder Buffer { get; set; }

        public void PushMessage(string message, MessageLevel level)
        {
            Console.WriteLine(message);
        }
    }
}
