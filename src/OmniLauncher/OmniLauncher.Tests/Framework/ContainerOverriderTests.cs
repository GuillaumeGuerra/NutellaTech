using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;

namespace OmniLauncher.Tests.Framework
{
    [TestFixture]
    public class ContainerOverriderTests
    {
        [Test]
        public void ShouldOverrideCurrentContainerAndRestoreItAfterTheDispose()
        {
            var initialContainer = App.Container;

            var newContainer = new ContainerBuilder().Build();
            using (ContainerOverrider.Override(newContainer))
            {
                Assert.That(App.Container, Is.SameAs(newContainer));
            }

            // And now it should be back to normal
            Assert.That(App.Container, Is.SameAs(initialContainer));
        }
    }
}
