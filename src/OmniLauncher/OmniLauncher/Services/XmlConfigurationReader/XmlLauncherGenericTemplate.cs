using System.Collections.Generic;
using System.Xml.Serialization;

namespace OmniLauncher.Services.XmlConfigurationReader
{
    [XmlRoot("GenericTemplate")]
    public class XmlLauncherGenericTemplate
    {
        [XmlArray("LinkGroups")]
        [XmlArrayItem("LinkGroup")]
        public List<XmlLauncherLinkGroup> LinkGroups { get; set; }
    }
}