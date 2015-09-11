using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDbgLibrary
{
    public class ThreadStackDeltaComputer
    {
        public ThreadStackDeltaComputation Compare(List<RunningThread> previous, List<RunningThread> current)
        {
            var delta = new ThreadStackDeltaComputation();

            var previousDico = previous.ToDictionary(thread => thread.ThreadId);
            var currentDico = previous.ToDictionary(thread => thread.ThreadId);

            foreach (var thread in current)
            {
                RunningThread previousThread;
                if (previousDico.TryGetValue(thread.ThreadId, out previousThread))
                {
                    thread.DeltaState = DeltaState.StillRunning;
                    thread.CpuTime = thread.CpuTime - previousThread.CpuTime;
                    thread.KernelTime = thread.KernelTime - previousThread.KernelTime;

                    previousDico.Remove(thread.ThreadId);
                    delta.StillAliveThreads.Add(thread);
                }
                else
                {
                    thread.DeltaState = DeltaState.New;
                    delta.NewThreads.Add(thread);
                }
            }

            foreach (var previousThread in previousDico.Values)
            {
                previousThread.DeltaState = DeltaState.Terminated;
                delta.TerminatedThreads.Add(previousThread);
            }

            return delta;
        }
    }
}
