using System;

namespace PatchManager.TestFramework.Context
{
    public static class TestContextExtensions
    {
        public static PatchManagerContextMock WithFrozenSystemDate(this PatchManagerContextMock context, DateTime now)
        {
            context.NowProperty.Override(now);
            return context;
        }

        public static PatchManagerContextMock WithFrozenSystemDate(this PatchManagerContextMock context)
        {
            return WithFrozenSystemDate(context, DateTime.Now);
        }
    }
}