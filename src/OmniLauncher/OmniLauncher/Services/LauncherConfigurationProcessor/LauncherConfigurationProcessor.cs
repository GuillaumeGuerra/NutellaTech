using OmniLauncher.Services.XmlConfigurationReader;

namespace OmniLauncher.Services.LauncherConfigurationProcessor
{
    public class LauncherConfigurationProcessor
    {
        private const string RootToken = "[ROOT]";

        public Launchers ProcessConfiguration(XmlLauncherConfiguration configuration)
        {
            var launchers = new Launchers();

            foreach (var root in configuration.RootDirectories)
            {
                launchers.RootGroups.Add(ProcessRoot(root, configuration.GenericTemplate));
            }

            return launchers;
        }

        public LaunchersRootGroup ProcessRoot(XmlLauncherRootDirectory root, XmlLauncherGenericTemplate template)
        {
            var result = new LaunchersRootGroup() { Header = root.Header };

            foreach (var group in template.LinkGroups)
            {
                result.Groups.Add(ProcessGroup(root, group));
            }

            return result;
        }

        public LaunchersGroup ProcessGroup(XmlLauncherRootDirectory root, XmlLauncherLinkGroup group)
        {
            var result = new LaunchersGroup() { Header = group.Header };

            foreach (var launcher in group.Launchers)
            {
                result.Launchers.Add(ProcessLauncherLink(root, launcher));
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