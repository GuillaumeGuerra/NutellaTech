using System.Collections.Generic;

namespace OmniLauncher.Services.LauncherConfigurationProcessor
{
    public class LaunchersGroup
    {
        public string Header { get; set; }
        public List<LauncherLink> Launchers { get; set; }

        public LaunchersGroup()
        {
            Launchers = new List<LauncherLink>();
        }
    }
}