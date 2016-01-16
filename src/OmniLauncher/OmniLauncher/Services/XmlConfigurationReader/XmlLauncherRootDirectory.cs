using System.Xml.Serialization;

namespace OmniLauncher.Services.XmlConfigurationReader
{
    public class XmlLauncherRootDirectory
    {
        [XmlAttribute]
        public string Path { get; set; }

        [XmlAttribute]
        public string Header { get; set; }
    }
}