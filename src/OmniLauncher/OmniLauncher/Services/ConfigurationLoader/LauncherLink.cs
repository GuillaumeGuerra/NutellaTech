using System.Collections.Generic;

namespace OmniLauncher.Services.ConfigurationLoader
{
    public class LauncherLink
    {
        public string Header { get; set; }
        public List<LauncherCommand> Commands { get; set; }
    }
}