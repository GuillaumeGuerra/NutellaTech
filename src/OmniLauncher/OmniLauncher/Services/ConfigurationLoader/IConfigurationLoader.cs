using System.Collections;
using System.Collections.Generic;

namespace OmniLauncher.Services.ConfigurationLoader
{
    public interface IConfigurationLoader
    {
        IEnumerable<LaunchersNode> LoadConfiguration(string path);
    }
}