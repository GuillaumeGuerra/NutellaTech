using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StarLauncher.Framework
{
    public static class ObjectFactory
    {
        private static CompositionContainer container { get; set; }
        private static AssemblyCatalog catalog { get; set; }

        static ObjectFactory()
        {
            catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            container = new CompositionContainer(catalog);
        }

        public static void ProcessDependencies<T>(T obj)
        {
            container.ComposeParts(obj);
        }

        public static T GetInstance<T>()
        {
            return container.GetExportedValue<T>();
        }

        public static IEnumerable<T> GetAllInstances<T>()
        {
            return container.GetExportedValues<T>();
        }
    }
}
