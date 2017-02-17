using System.Xml.Serialization;

namespace OmniLauncher.Services.ConfigurationLoader.Xml
{
    public class XmlLauncherRootDirectory
    {
        [XmlAttribute]
        public string Path { get; set; }

        [XmlAttribute]
        public string Header { get; set; }
    }
}