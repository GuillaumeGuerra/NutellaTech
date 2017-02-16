using System.Collections.Generic;
using System.Xml.Serialization;

namespace OmniLauncher.Services.XmlConfigurationReader
{
    public class XmlLauncherLink
    {
        [XmlAttribute]
        public string Header { get; set; }

        [XmlArray("Commands")]
        [XmlArrayItem("Execute", typeof(XmlExecuteCommand))]
        [XmlArrayItem("XPath", typeof(XmlXPathReplacerCommand))]
        public List<XmlLauncherCommand> Commands { get; set; }
    }
}