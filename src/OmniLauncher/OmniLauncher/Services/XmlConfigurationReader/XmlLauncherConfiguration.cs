﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace OmniLauncher.Services.XmlConfigurationReader
{
    [XmlRoot("Configuration")]
    public class XmlLauncherConfiguration
    {
        [XmlArray("RootDirectories")]
        [XmlArrayItem("RootDirectory")]
        public List<XmlLauncherRootDirectory> RootDirectories { get; set; }
        
        public XmlLauncherGenericTemplate GenericTemplate { get; set; }
    }
}