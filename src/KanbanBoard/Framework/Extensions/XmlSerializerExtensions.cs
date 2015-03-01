using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Framework.Extensions
{
    public static class XmlSerializerExtensions
    {
        public static string Serialize(this XmlSerializer serializer, object o)
        {
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, o);

                return writer.GetStringBuilder().ToString();
            }
        }

        public static T Deserialize<T>(this XmlSerializer serializer, string xml)
        {
            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
