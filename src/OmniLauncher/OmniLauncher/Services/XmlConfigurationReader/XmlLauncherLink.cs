using System.Xml.Serialization;

namespace OmniLauncher.Services.XmlConfigurationReader
{
    public class XmlLauncherLink
    {
        [XmlAttribute]
        public string Header { get; set; }

        [XmlAttribute]
        public string Command { get; set; }
    }
}