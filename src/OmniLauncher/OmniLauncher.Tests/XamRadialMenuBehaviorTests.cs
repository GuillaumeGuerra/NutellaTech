using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infragistics.Controls.Menus;
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
            var actual = new XamRadialMenuBehavior().GetMenuItems(nullLaunchers ? null : new LaunchersNode());
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(0));
        }

        [Test]
        [STAThread]
        public void ShouldBuildRecursiveMenuItemsWhenParsingLaunchers()
        {
            var launchers = new LaunchersNode()
            {
                SubGroups = new List<LaunchersNode>()
                {
                    new LaunchersNode()
                    {
                        Header = "Item1",
                        Launchers = new List<LauncherLink>()
                        {
                            new LauncherLink()
                            {
                                Header = "Button1",
                                Command = "Command1"
                            }
                        },
                        SubGroups = new List<LaunchersNode>()
                        {
                            new LaunchersNode()
                            {
                                Header = "Item1-1",
                                Launchers = new List<LauncherLink>()
                                {
                                    new LauncherLink()
                                    {
                                        Header = "Button1-1",
                                        Command = "Command1-1"
                                    }
                                }
                            }
                        }
                    },
                    new LaunchersNode()
                    {
                        Header = "Item2",
                        Launchers = new List<LauncherLink>()
                        {
                            new LauncherLink()
                            {
                                Header = "Button2",
                                Command = "Command2"
                            }
                        }
                    }
                }
            };
            var actual = new XamRadialMenuBehavior().GetMenuItems(launchers);

            Assert.That(actual, Has.Count.EqualTo(2));

            var first = actual[0];
            Assert.That(first.Header, Is.EqualTo("Item1"));
            Assert.That(first.Items, Has.Count.EqualTo(2));

            var firstChild = first.Items[0] as RadialMenuItem;
            Assert.That(firstChild, Is.Not.Null);
            Assert.That(firstChild.Header, Is.EqualTo("Item1-1"));
            Assert.That(firstChild.Items, Has.Count.EqualTo(1));
            Assert.That((firstChild.Items[0] as RadialMenuItem).Header, Is.EqualTo("Button1-1"));

            var firstSubChild = first.Items[1] as RadialMenuItem;
            Assert.That(firstSubChild, Is.Not.Null);
            Assert.That(firstSubChild.Header, Is.EqualTo("Button1"));

            var second = actual[1];
            Assert.That(second.Header, Is.EqualTo("Item2"));
            Assert.That(second.Items, Has.Count.EqualTo(1));

            var secondChild = second.Items[0] as RadialMenuItem;
            Assert.That(secondChild, Is.Not.Null);
            Assert.That(secondChild.Header, Is.EqualTo("Button2"));
        }
    }
}
