using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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

            Assert.That(actual.RootGroups, Is.Not.Null);
            Assert.That(actual.RootGroups.Count, Is.EqualTo(2));

            AssertRootGroupContent(actual.RootGroups[0], "D:/Jar-Jar You Stink", "Jar-Jar");
            AssertRootGroupContent(actual.RootGroups[1], "E:/Star Trek/Kirk_you_suck", "Kirk");
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

        private void AssertRootGroupContent(LaunchersRootGroup rootGroup, string rootDirectory, string expectedHeader)
        {
            Assert.That(rootGroup.Header, Is.EqualTo(expectedHeader));

            Assert.That(rootGroup.Groups, Is.Not.Null);
            Assert.That(rootGroup.Groups.Count, Is.EqualTo(2));

            Assert.That(rootGroup.Groups[0], Is.Not.Null);
            Assert.That(rootGroup.Groups[0].Header, Is.EqualTo("Solutions"));

            Assert.That(rootGroup.Groups[0].Launchers, Is.Not.Null);
            Assert.That(rootGroup.Groups[0].Launchers.Count, Is.EqualTo(3));

            Assert.That(rootGroup.Groups[0].Launchers[0].Header, Is.EqualTo("Base.sln"));
            Assert.That(rootGroup.Groups[0].Launchers[0].Command, Is.EqualTo(rootDirectory + "/Rebels/Yavin/base.sln"));

            Assert.That(rootGroup.Groups[0].Launchers[1].Header, Is.EqualTo("Padawan.sln"));
            Assert.That(rootGroup.Groups[0].Launchers[1].Command, Is.EqualTo(rootDirectory + "/Jedis/padawan.sln"));

            Assert.That(rootGroup.Groups[0].Launchers[2].Header, Is.EqualTo("Stormtrooper.sln"));
            Assert.That(rootGroup.Groups[0].Launchers[2].Command, Is.EqualTo("C:/Empire/stormtrooper.sln"));

            Assert.That(rootGroup.Groups[1], Is.Not.Null);
            Assert.That(rootGroup.Groups[1].Header, Is.EqualTo("Launchers"));

            Assert.That(rootGroup.Groups[1].Launchers, Is.Not.Null);
            Assert.That(rootGroup.Groups[1].Launchers.Count, Is.EqualTo(1));

            Assert.That(rootGroup.Groups[1].Launchers[0].Header, Is.EqualTo("Rebellion"));
            Assert.That(rootGroup.Groups[1].Launchers[0].Command,
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
                GenericTemplate = new XmlLauncherGenericTemplate()
                {
                    LinkGroups = new List<XmlLauncherLinkGroup>()
                    {
                        new XmlLauncherLinkGroup()
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
                        new XmlLauncherLinkGroup()
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

    public class LauncherConfigurationProcessor
    {
        private const string RootToken = "[ROOT]";

        public Launchers ProcessConfiguration(XmlLauncherConfiguration configuration)
        {
            var launchers = new Launchers();

            foreach (var root in configuration.RootDirectories)
            {
                launchers.RootGroups.Add(ProcessRoot(root, configuration.GenericTemplate));
            }

            return launchers;
        }

        public LaunchersRootGroup ProcessRoot(XmlLauncherRootDirectory root, XmlLauncherGenericTemplate template)
        {
            var result = new LaunchersRootGroup() { Header = root.Header };

            foreach (var group in template.LinkGroups)
            {
                result.Groups.Add(ProcessGroup(root, group));
            }

            return result;
        }

        public LaunchersGroup ProcessGroup(XmlLauncherRootDirectory root, XmlLauncherLinkGroup group)
        {
            var result = new LaunchersGroup() { Header = group.Header };

            foreach (var launcher in group.Launchers)
            {
                result.Launchers.Add(ProcessLauncherLink(root, launcher));
            }

            return result;
        }

        public LauncherLink ProcessLauncherLink(XmlLauncherRootDirectory root, XmlLauncherLink launcher)
        {
            var resolvedRootPath = root.Path;
            if (root.Path.EndsWith("/") || root.Path.EndsWith(@"\"))
                resolvedRootPath = resolvedRootPath.Substring(0, resolvedRootPath.Length - 1);

            return new LauncherLink()
            {
                Header = launcher.Header,
                Command = launcher.Command.Replace(RootToken, resolvedRootPath)
            };
        }
    }

    public class Launchers
    {
        public List<LaunchersRootGroup> RootGroups { get; set; }

        public Launchers()
        {
            RootGroups = new List<LaunchersRootGroup>();
        }
    }

    public class LaunchersRootGroup
    {
        public string Header { get; set; }

        public List<LaunchersGroup> Groups { get; set; }

        public LaunchersRootGroup()
        {
            Groups = new List<LaunchersGroup>();
        }
    }

    public class LaunchersGroup
    {
        public string Header { get; set; }
        public List<LauncherLink> Launchers { get; set; }

        public LaunchersGroup()
        {
            Launchers = new List<LauncherLink>();
        }
    }

    public class LauncherLink
    {
        public string Header { get; set; }
        public string Command { get; set; }
    }
}
