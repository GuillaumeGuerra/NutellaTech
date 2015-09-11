using System.Collections.Generic;

namespace OneDbgLibrary
{
    public class ThreadStackDeltaComputation
    {
        public List<RunningThread> StillAliveThreads { get; set; }
        public List<RunningThread> NewThreads { get; set; }
        public List<RunningThread> TerminatedThreads { get; set; }

        public ThreadStackDeltaComputation()
        {
            StillAliveThreads = new List<RunningThread>();
            NewThreads = new List<RunningThread>();
            TerminatedThreads = new List<RunningThread>();
        }
    }
}