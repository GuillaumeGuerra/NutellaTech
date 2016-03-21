using System;

namespace PatchManager.Services.Tests.Framework
{
    public class Overridable<T>
    {
        public Overridable(Func<T> func)
        {
            IsValueOverriden = false;
            Func = func;
        }

        public T Value => IsValueOverriden ? OverridenValue : Func();

        private bool IsValueOverriden { get; set; }
        private T OverridenValue { get; set; }
        private Func<T> Func { get; }

        public void Override(T value)
        {
            OverridenValue = value;
            IsValueOverriden = true;
        }
    }
}