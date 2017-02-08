using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OmniLauncher.Services.LauncherConfigurationProcessor;
using OmniLauncher.Services.XmlConfigurationReader;

namespace OmniLauncher.Tests
{
    [TestFixture]
    public class LauncherConfigurationProcessorTests
    {
        [Test]
        public void ShouldReplaceRootTokenWithRootDirectory()
        {
            var processor = new LauncherConfigurationProcessor();
            var actual = processor.ProcessConfiguration(GetTemplateConfiguration());

            Assert.That(actual.SubGroups, Is.Not.Null);
            Assert.That(actual.SubGroups.Count, Is.EqualTo(2));

            AssertNode(actual.SubGroups[0], "D:/Jar-Jar You Stink", "Jar-Jar");
            AssertNode(actual.SubGroups[1], "E:/Star Trek/Kirk_you_suck", "Kirk");
        }

        [TestCase("root/path/with/slashes", "[ROOT]/end/of/path", "root/path/with/slashes/end/of/path", Description = "Genuine case")]
        [TestCase("root/path/with/slashes/", "[ROOT]/end/of/path", "root/path/with/slashes/end/of/path", Description = "Extra / after the root")]
        [TestCase(@"root\path\with\backslashes", @"[ROOT]\end\of\path", @"root\path\with\backslashes\end\of\path", Description = "Genuine case")]
        [TestCase(@"root\path\with\backslashes\", @"[ROOT]\end\of\path", @"root\path\with\backslashes\end\of\path", Description = @"Extra \ after the root")]
        public void ShouldProcessSlashAndBackSlashWhenReplacingRootToken(string path, string command, string expected)
        {
            var processor = new LauncherConfigurationProcessor();
            var actual = processor.ProcessLauncherLink(new XmlLauncherRootDirectory()
            {
                Path = path
            },
            new XmlLauncherLink()
            {
                Command = command
            });

            Assert.That(actual.Command, Is.EqualTo(expected));
        }

        private void AssertNode(LaunchersNode rootGroup, string rootDirectory, string expectedHeader)
        {
            Assert.That(rootGroup.Header, Is.EqualTo(expectedHeader));

            Assert.That(rootGroup.SubGroups, Is.Not.Null);
            Assert.That(rootGroup.SubGroups.Count, Is.EqualTo(2));

            Assert.That(rootGroup.SubGroups[0], Is.Not.Null);
            Assert.That(rootGroup.SubGroups[0].Header, Is.EqualTo("Solutions"));

            Assert.That(rootGroup.SubGroups[0].Launchers, Is.Not.Null);
            Assert.That(rootGroup.SubGroups[0].Launchers.Count, Is.EqualTo(3));

            Assert.That(rootGroup.SubGroups[0].Launchers[0].Header, Is.EqualTo("Base.sln"));
            Assert.That(rootGroup.SubGroups[0].Launchers[0].Command, Is.EqualTo(rootDirectory + "/Rebels/Yavin/base.sln"));

            Assert.That(rootGroup.SubGroups[0].Launchers[1].Header, Is.EqualTo("Padawan.sln"));
            Assert.That(rootGroup.SubGroups[0].Launchers[1].Command, Is.EqualTo(rootDirectory + "/Jedis/padawan.sln"));

            Assert.That(rootGroup.SubGroups[0].Launchers[2].Header, Is.EqualTo("Stormtrooper.sln"));
            Assert.That(rootGroup.SubGroups[0].Launchers[2].Command, Is.EqualTo("C:/Empire/stormtrooper.sln"));

            Assert.That(rootGroup.SubGroups[1], Is.Not.Null);
            Assert.That(rootGroup.SubGroups[1].Header, Is.EqualTo("Launchers"));

            Assert.That(rootGroup.SubGroups[1].Launchers, Is.Not.Null);
            Assert.That(rootGroup.SubGroups[1].Launchers.Count, Is.EqualTo(1));

            Assert.That(rootGroup.SubGroups[1].Launchers[0].Header, Is.EqualTo("Rebellion"));
            Assert.That(rootGroup.SubGroups[1].Launchers[0].Command,
                Is.EqualTo(rootDirectory + "/Rebels/Yavin/bin/debug/Start rebellion.cmd"));
        }

        private XmlLauncherConfiguration GetTemplateConfiguration()
        {
            var configuration = new XmlLauncherConfiguration
            {
                RootDirectories = new List<XmlLauncherRootDirectory>()
                {
                    new XmlLauncherRootDirectory()
                    {
                        Header = "Jar-Jar",
                        Path = "D:/Jar-Jar You Stink"
                    },
                    new XmlLauncherRootDirectory()
                    {
                        Header = "Kirk",
                        Path = "E:/Star Trek/Kirk_you_suck"
                    }
                },
                GenericTemplate = new XmlLauncherNode()
                {
                    SubGroups = new List<XmlLauncherNode>()
                    {
                        new XmlLauncherNode()
                        {
                            Header = "Solutions",
                            Launchers = new List<XmlLauncherLink>()
                            {
                                new XmlLauncherLink()
                                {
                                    Header = "Base.sln",
                                    Command = "[ROOT]/Rebels/Yavin/base.sln"
                                },
                                new XmlLauncherLink()
                                {
                                    Header = "Padawan.sln",
                                    Command = "[ROOT]/Jedis/padawan.sln"
                                },
                                new XmlLauncherLink()
                                {
                                    Header = "Stormtrooper.sln",
                                    Command = "C:/Empire/stormtrooper.sln"
                                }
                            }
                        },
                        new XmlLauncherNode()
                        {
                            Header = "Launchers",
                            Launchers = new List<XmlLauncherLink>()
                            {
                                new XmlLauncherLink()
                                {
                                    Header = "Rebellion",
                                    Command = "[ROOT]/Rebels/Yavin/bin/debug/Start rebellion.cmd"
                                }
                            }
                        }
                    }
                }
            };
            return configuration;
        }
    }
}
