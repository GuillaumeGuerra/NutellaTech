using System.Xml.Serialization;
using OmniLauncher.Services.LauncherConfigurationProcessor;

namespace OmniLauncher.Services.XmlConfigurationReader
{
    public class XmlExecuteCommand : XmlLauncherCommand
    {
        [XmlAttribute]
        public string Command { get; set; }

        public override LauncherCommand ToCommand(string resolvedRootPath)
        {
            return new ExecuteCommand() { Command = Command.Replace(RootToken, resolvedRootPath) };
        }
    }
}