using System.Collections.Generic;
using System.Windows.Documents;

namespace OmniLauncher.Services.LauncherConfigurationProcessor
{
    public class LauncherLink
    {
        public string Header { get; set; }
        public List<LauncherCommand> Commands { get; set; }
    }
}