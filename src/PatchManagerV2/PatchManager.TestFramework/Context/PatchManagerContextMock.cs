using System;
using PatchManager.Config;
using PatchManager.Services.Context;

namespace PatchManager.TestFramework.Context
{
    public class PatchManagerContextMock : IPatchManagerContextService
    {
        public DateTime Now => NowProperty.Value;
        public SettingsConfiguration Settings => SettingsProperty.Value;

        internal readonly Overridable<DateTime> NowProperty = new Overridable<DateTime>(() => DateTime.Now);

        internal readonly Overridable<SettingsConfiguration> SettingsProperty = new Overridable<SettingsConfiguration>(() => new SettingsConfiguration()
        {
            TimeoutInMinutesToResolveGerritStatus = -5,
            GerritUrl = "http://yodaisold.com",
            JiraUrl = "http://obiwantoo.com",
            PersistenceDirectoryPath = "d:/here_is_jar-jar_,_please_get_rid_of_the_moron"
        });
    }
}
