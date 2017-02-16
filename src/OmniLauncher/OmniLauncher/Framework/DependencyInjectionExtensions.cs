using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using OmniLauncher.Services.CommandLauncher;

namespace OmniLauncher.Framework
{
    public static class DependencyInjectionExtensions
    {
        public static IEnumerable<TService> GetImplementations<TService>(this IContainer container)
        {
            return container.Resolve<IEnumerable<TService>>();
        }
    }
}
