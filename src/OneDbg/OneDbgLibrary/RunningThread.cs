using System.Collections.Generic;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace OneDbgLibrary
{
    public class RunningThread
    {
        public uint ThreadId { get; set; }
        public List<StackFrame> Stack { get; set; }
        public uint LockCount { get; set; }
        public bool IsWaiting { get; set; }
        public string CurrentFrame { get; set; }
        public int StackHashCode { get; set; }

        public RunningThread(ClrThread thread)
        {
            ThreadId = thread.OSThreadId;
            LockCount = thread.LockCount;
            IsWaiting = IsThreadWaiting(thread);

            int frameIndex = 0;
            Stack = thread.StackTrace.Select(frame => new StackFrame()
            {
                FrameIndex = frameIndex++,
                InstructionPointer = frame.InstructionPointer,
                StackPointer = frame.StackPointer,
                DisplayString = frame.ToString()
            }).ToList();

            if (Stack.Count > 0)
            {
                CurrentFrame = Stack.First().DisplayString;
                StackHashCode = string.Join("---", Stack).GetHashCode();
            }
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
}