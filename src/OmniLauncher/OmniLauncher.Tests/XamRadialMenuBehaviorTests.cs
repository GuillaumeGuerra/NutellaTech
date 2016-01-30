using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OmniLauncher.Behaviors;
using OmniLauncher.Services.LauncherConfigurationProcessor;

namespace OmniLauncher.Tests
{
    [TestFixture]
    public class XamRadialMenuBehaviorTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldCreateEmptyListOfItemsWhenBoundToEmptyListOfLaunchers(bool nullLaunchers)
        {
            var behavior = new XamRadialMenuBehavior();

            var actual = behavior.GetRadialMenuItems(nullLaunchers ? null : new Launchers());
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(0));
        }
    }
}
