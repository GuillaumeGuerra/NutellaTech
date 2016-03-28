using System;
using PatchManager.Config;

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

        public static PatchManagerContextMock WithSettings(this PatchManagerContextMock context, SettingsConfiguration settings)
        {
            context.SettingsProperty.Override(settings);
            return context;
        }
    }
}