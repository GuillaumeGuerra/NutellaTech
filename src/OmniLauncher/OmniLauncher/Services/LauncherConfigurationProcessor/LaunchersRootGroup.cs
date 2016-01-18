using System.Collections.Generic;

namespace OmniLauncher.Services.LauncherConfigurationProcessor
{
    public class LaunchersRootGroup
    {
        public string Header { get; set; }

        public List<LaunchersGroup> Groups { get; set; }

        public LaunchersRootGroup()
        {
            Groups = new List<LaunchersGroup>();
        }
    }
}