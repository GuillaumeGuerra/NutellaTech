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
            using (DataTarget dataTarget = DataTarget.AttachToProcess(ProcessPID, 5000))
            {
                string dacLocation = dataTarget.ClrVersions[0].TryGetDacLocation();
                var runtime = dataTarget.CreateRuntimeHack(dacLocation, 4, 5);

                return 
                    (from thread in runtime.Threads
                    where thread.StackTrace != null && thread.StackTrace.Count != 0
                    select new RunningThread(thread)).ToList();
            }
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
