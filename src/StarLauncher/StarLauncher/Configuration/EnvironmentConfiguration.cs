using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace StarLauncher.Configuration
{
    public class EnvironmentConfiguration : ConfigurationElement
    {
        public void Read(string xmlPath)
        {
            using (XmlTextReader reader = new XmlTextReader(xmlPath))
            {
                reader.Read();
                base.DeserializeElement(reader, false);
            }
        }

        [ConfigurationProperty("name")]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("image")]
        public string Image
        {
            get { return this["image"] as string; }
            set { this["image"] = value; }
        }

        [ConfigurationProperty("executable")]
        public string Executable
        {
            get { return this["executable"] as string; }
            set { this["executable"] = value; }
        }

        //C:\Users\Guillaume\Documents\Visual Studio 2013\Projects\StarLauncher\homeware
        [ConfigurationProperty("homewareRoot", DefaultValue = @"C:\Users\Guillaume\Documents\Visual Studio 2013\Projects\StarLauncher\homeware", IsRequired = false)]
        public string HomewareRoot
        {
            get { return this["homewareRoot"] as string; }
            set { this["homewareRoot"] = value; }
        }
       
        [ConfigurationProperty("directoryName", IsRequired = false)]
        public string DirectoryName
        {
            get { return this["directoryName"] as string; }
            set { this["directoryName"] = value; }
        }
    }
}
