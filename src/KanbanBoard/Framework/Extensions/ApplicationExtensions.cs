using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Framework.Extensions
{
    public static class ApplicationExtensions
    {
        public static T GetResource<T>(this Application app, string resourceKey)
            where T : class
        {
            return app.FindResource(resourceKey) as T;
        }
    }
}
