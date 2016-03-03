using System;

namespace PatchManager.Services.GerritActions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GerritActionAttribute : Attribute
    {
        public GerritActionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}