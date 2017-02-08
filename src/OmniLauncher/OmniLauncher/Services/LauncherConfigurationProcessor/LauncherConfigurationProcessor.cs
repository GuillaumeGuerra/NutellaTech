using OmniLauncher.Services.XmlConfigurationReader;

namespace OmniLauncher.Services.LauncherConfigurationProcessor
{
    public class LauncherConfigurationProcessor
    {
        private const string RootToken = "[ROOT]";

        public LaunchersNode ProcessConfiguration(XmlLauncherConfiguration configuration)
        {
            var launchers = new LaunchersNode();

            foreach (var root in configuration.RootDirectories)
            {
                launchers.SubGroups.Add(ProcessNode(configuration.GenericTemplate, root, root.Header));
            }

            return launchers;
        }

        private LaunchersNode ProcessNode(XmlLauncherNode node, XmlLauncherRootDirectory root, string header = null)
        {
            var result = new LaunchersNode() { Header = header ?? node.Header };

            if (node.SubGroups != null)
            {
                foreach (var group in node.SubGroups)
                {
                    result.SubGroups.Add(ProcessNode(group, root));
                }
            }

            if (node.Launchers != null)
            {
                foreach (var launcher in node.Launchers)
                {
                    result.Launchers.Add(ProcessLauncherLink(root, launcher));
                }
            }

            return result;
        }

        public LauncherLink ProcessLauncherLink(XmlLauncherRootDirectory root, XmlLauncherLink launcher)
        {
            var resolvedRootPath = root.Path;
            if (root.Path.EndsWith("/") || root.Path.EndsWith(@"\"))
                resolvedRootPath = resolvedRootPath.Substring(0, resolvedRootPath.Length - 1);

            return new LauncherLink()
            {
                Header = launcher.Header,
                Command = launcher.Command.Replace(RootToken, resolvedRootPath)
            };
        }
    }
}