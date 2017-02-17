using System.Xml.Serialization;
using OmniLauncher.Services.LauncherConfigurationProcessor;

namespace OmniLauncher.Services.XmlConfigurationReader
{
    public class XmlFileCopierCommand : XmlLauncherCommand
    {
        [XmlAttribute]
        public string SourceFilePath { get; set; }

        [XmlAttribute]
        public string TargetFilePath { get; set; }

        public override LauncherCommand ToCommand(string resolvedRootPath)
        {
            return new FileCopierCommand()
            {
                SourceFilePath = SourceFilePath.Replace(RootToken, resolvedRootPath),
                TargetFilePath = TargetFilePath.Replace(RootToken, resolvedRootPath)
            };
        }
    }
}