using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PatchManager.Framework
{
    public class WaitHelper
    {
        public static void Sleep(int minSeconds = 2, int maxSeconds = 4)
        {
            Thread.Sleep((new Random((int)DateTime.Now.Ticks).Next(maxSeconds - minSeconds) + minSeconds) * 1000);
        }
    }
}
