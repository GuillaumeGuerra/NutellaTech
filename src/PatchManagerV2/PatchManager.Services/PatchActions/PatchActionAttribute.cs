using System;

namespace PatchManager.Services.PatchActions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PatchActionAttribute : Attribute
    {
        public PatchActionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}