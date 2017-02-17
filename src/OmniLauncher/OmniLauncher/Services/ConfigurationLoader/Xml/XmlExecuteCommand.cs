using System.Xml.Serialization;
using OmniLauncher.Services.CommandLauncher;

namespace OmniLauncher.Services.ConfigurationLoader.Xml
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