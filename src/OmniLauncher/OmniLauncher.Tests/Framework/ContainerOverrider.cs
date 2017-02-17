using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace OmniLauncher.Tests.Framework
{
    public class ContainerOverrider : IDisposable
    {
        private IContainer _originalContainer;

        public ContainerOverrider()
        {
            _originalContainer = App.Container;
        }

        public void Dispose()
        {
            // If the container was overriden, we'll dispose it before reverting to the original one
            if (App.Container != null && !ReferenceEquals(App.Container, _originalContainer))
                App.Container.Dispose();

            App.Container = _originalContainer;
        }
    }
}
