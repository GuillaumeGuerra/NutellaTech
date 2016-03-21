using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatchManager.Config;
using PatchManager.Services.Context;

namespace PatchManager.Services.Tests.Framework
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
            JiraUrl = "http://obiwantoo.com"
        });
    }
}
