using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmniLauncher.Services.LauncherConfigurationProcessor
{
    public class Launchers
    {
        public List<LaunchersRootGroup> RootGroups { get; set; }

        public Launchers()
        {
            RootGroups = new List<LaunchersRootGroup>();
        }

        public static implicit operator Launchers(Task<Launchers> v)
        {
            throw new NotImplementedException();
        }
    }
}