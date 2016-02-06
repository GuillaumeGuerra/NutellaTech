using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatchManager.Config
{
    public class SettingsConfiguration
    {
        public int TimeoutInMinutesToResolveGerritStatus { get; set; }

        public string JiraUrl { get; set; }

        public string GerritUrl { get; set; }
    }
}
