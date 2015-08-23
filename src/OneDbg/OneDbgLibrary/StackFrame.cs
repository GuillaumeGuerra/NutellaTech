namespace OneDbgLibrary
{
    public class StackFrame
    {
        public int FrameIndex { get; set; }
        public string DisplayString { get; set; }
        public ulong StackPointer { get; set; }
        public ulong InstructionPointer { get; set; }
    }
}