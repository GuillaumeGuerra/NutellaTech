using System.Collections.Generic;
using System.Xml.Serialization;

namespace OmniLauncher.Services.XmlConfigurationReader
{
    public class XmlLauncherLinkGroup
    {
        [XmlArray("Launchers")]
        [XmlArrayItem("Launcher")]
        public List<XmlLauncherLink> Launchers { get; set; }

        [XmlAttribute]
        public string Header { get; set; }
    }
}