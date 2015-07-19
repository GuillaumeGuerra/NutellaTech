namespace OneDbgLibrary
{
    public class StackFrame
    {
        public string DisplayString { get; set; }
        public ulong StackPointer { get; set; }
        public ulong InstructionPointer { get; set; }
    }
}