using System.Collections.Generic;

namespace OneDbgLibrary
{
    public class RunningThread
    {
        public uint ThreadId { get; set; }
        public List<StackFrame> Stack { get; set; }
        public uint LockCount { get; set; }
        public bool IsWaiting { get; set; }
        public string Name { get; set; }
    }
}