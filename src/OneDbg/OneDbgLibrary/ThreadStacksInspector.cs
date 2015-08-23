using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                ClrRuntime runtime = dataTarget.CreateRuntimeHack(dacLocation, 4, 5);

                foreach (ClrThread thread in runtime.Threads)
                {
                    if (thread.StackTrace == null || thread.StackTrace.Count == 0)
                        continue;

                    var runningThread = new RunningThread
                    {
                        ThreadId = thread.OSThreadId,
                        LockCount = thread.LockCount,
                        IsWaiting = IsThreadWaiting(thread),
                        Stack = thread.StackTrace.Select(frame => new StackFrame()
                        {
                            InstructionPointer = frame.InstructionPointer,
                            StackPointer = frame.StackPointer,
                            DisplayString = frame.ToString()
                        }).ToList()
                    };

                    if (runningThread.Stack.Count > 0)
                    {
                        runningThread.CurrentFrame = runningThread.Stack.First().DisplayString;
                        runningThread.StackHashCode = string.Join("---", runningThread.Stack).GetHashCode();
                    }

                    threads.Add(runningThread);
                }
            }

            return threads;
        }

        private bool IsThreadWaiting(ClrThread thread)
        {
            if (thread.StackTrace.Count > 1)
            {
                var frame = thread.StackTrace[1].DisplayString.ToUpper();
                return frame.Contains("WAIT") || frame.Contains("SLEEP");
            }

            if (thread.StackTrace.Count > 0)
            {
                var frame = thread.StackTrace[0].DisplayString.ToUpper();
                return frame.Contains("WAIT") || frame.Contains("SLEEP");
            }

            return false;
        }
    }

    public static class ClrMdExtensions
    {
        public static ClrRuntime CreateRuntimeHack(this DataTarget target, string dacLocation, int major, int minor)
        {
            string dacFileNoExt = Path.GetFileNameWithoutExtension(dacLocation);
            if (dacFileNoExt.Contains("mscordacwks") && major == 4 && minor >= 5)
            {
                Type dacLibraryType = typeof(DataTarget).Assembly.GetType("Microsoft.Diagnostics.Runtime.DacLibrary");
                object dacLibrary = Activator.CreateInstance(dacLibraryType, target, dacLocation);
                Type v45RuntimeType = typeof(DataTarget).Assembly.GetType("Microsoft.Diagnostics.Runtime.Desktop.V45Runtime");
                object runtime = Activator.CreateInstance(v45RuntimeType, target, dacLibrary);
                return (ClrRuntime)runtime;
            }
            else
            {
                return target.CreateRuntime(dacLocation);
            }
        }
    }
}
