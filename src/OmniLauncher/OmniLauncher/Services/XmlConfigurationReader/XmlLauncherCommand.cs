using System.Xml.Serialization;
using OmniLauncher.Services.LauncherConfigurationProcessor;

namespace OmniLauncher.Services.XmlConfigurationReader
{
    [XmlInclude(typeof(XmlExecuteCommand))]
    [XmlInclude(typeof(XmlXPathReplacerCommand))]
    public abstract class XmlLauncherCommand
    {
        protected const string RootToken = "[ROOT]";

        public abstract LauncherCommand ToCommand(string resolvedRootPath);
    }
}