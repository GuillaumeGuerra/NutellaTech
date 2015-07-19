using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime;

namespace OneDbgLibrary
{
    public class ThreadStacksInspector
    {
        public int ProcessPID { get; set; }

        public ThreadStacksInspector(int processPID)
        {
            ProcessPID = processPID;
        }

        public List<RunningThread> LoadStacks()
        {
            var threads = new List<RunningThread>();

            using (DataTarget dataTarget = DataTarget.AttachToProcess(ProcessPID, 5000))
            {
                string dacLocation = dataTarget.ClrVersions[0].TryGetDacLocation();
                ClrRuntime runtime = dataTarget.CreateRuntime(dacLocation);

                foreach (ClrThread thread in runtime.Threads)
                {
                    if (thread.StackTrace == null || thread.StackTrace.Count == 0)
                        continue;

                    var runningThread = new RunningThread
                    {
                        ThreadId = thread.OSThreadId,
                        LockCount = thread.LockCount,
                        IsWaiting = IsThreadWaiting(thread),
                        Name = "TODO",
                        Stack = thread.StackTrace.Select(frame => new StackFrame()
                        {
                            InstructionPointer = frame.InstructionPointer,
                            StackPointer = frame.StackPointer,
                            DisplayString = frame.ToString()
                        }).ToList()
                    };

                    threads.Add(runningThread);
                }
            }

            return threads;
        }

        private bool IsThreadWaiting(ClrThread thread)
        {
            return thread.StackTrace.Count > 0 && thread.StackTrace[0].DisplayString.ToUpper().Contains("WAIT") || thread.StackTrace.Count > 1 && thread.StackTrace[1].DisplayString.ToUpper().Contains("WAIT");
        }
    }
}
