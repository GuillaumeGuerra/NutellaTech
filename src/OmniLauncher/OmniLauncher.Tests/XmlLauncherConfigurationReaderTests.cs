using System;
using NUnit.Framework;
using OmniLauncher.Services.XmlConfigurationReader;

namespace OmniLauncher.Tests
{
    [TestFixture]
    public class XmlLauncherConfigurationReaderTests
    {
        [Test]
        public void ShouldReadAllRowsProperlyWhenLoadingXmlFile()
        {
            var service = new XmlLauncherConfigurationReader();
            var actual = service.LoadFile("Data/LaunchersTest1.xml");
            Assert.That(actual.RootDirectories, Is.Not.Null);
            Assert.That(actual.RootDirectories.Count, Is.EqualTo(2));

            Assert.That(actual.RootDirectories[0].Path, Is.EqualTo("D:/Jar-Jar You Stink"));
            Assert.That(actual.RootDirectories[0].Header, Is.EqualTo("Jar-Jar"));
            Assert.That(actual.RootDirectories[1].Path, Is.EqualTo("E:/Star Trek/Kirk_you_suck"));
            Assert.That(actual.RootDirectories[1].Header, Is.EqualTo("Kirk"));

            Assert.That(actual.GenericTemplate, Is.Not.Null);
            Assert.That(actual.GenericTemplate.Header, Is.Null); // No header for the template

            Assert.That(actual.GenericTemplate.Launchers, Is.Not.Null);
            Assert.That(actual.GenericTemplate.Launchers, Has.Count.EqualTo(1));

            AssertSingleCommandLauncher("Empire", "[ROOT]/Rebels/DeathStar/Fire.cmd", actual.GenericTemplate.Launchers[0]);

            Assert.That(actual.GenericTemplate.SubGroups, Is.Not.Null);
            Assert.That(actual.GenericTemplate.SubGroups, Has.Count.EqualTo(2));

            var firstGroup = actual.GenericTemplate.SubGroups[0];
            Assert.That(firstGroup.Header, Is.EqualTo("Solutions"));

            Assert.That(firstGroup.Launchers, Is.Not.Null);
            Assert.That(firstGroup.Launchers, Has.Count.EqualTo(2));

            Assert.That(firstGroup.Launchers[0], Is.Not.Null);
            Assert.That(firstGroup.Launchers[0].Header, Is.EqualTo("Base.sln"));
            Assert.That(firstGroup.Launchers[0].Commands, Has.Count.EqualTo(2));
            // First, the Execute command
            Assert.That(((XmlExecuteCommand)firstGroup.Launchers[0].Commands[0]).Command, Is.EqualTo("[ROOT]/Rebels/Yavin/base.sln"));
            // And then comes the XPath replacer
            Assert.That(((XmlXPathReplacerCommand)firstGroup.Launchers[0].Commands[1]).XPath, Is.EqualTo("configuration/appSettings[@name='who's the best jedi ?']"));
            Assert.That(((XmlXPathReplacerCommand)firstGroup.Launchers[0].Commands[1]).Value, Is.EqualTo("yoda"));

            AssertSingleCommandLauncher("Padawan.sln", "[ROOT]/Jedis/padawan.sln", firstGroup.Launchers[1]);

            Assert.That(firstGroup.SubGroups, Is.Not.Null);
            Assert.That(firstGroup.SubGroups, Has.Count.EqualTo(0));

            var secondGroup = actual.GenericTemplate.SubGroups[1];
            Assert.That(secondGroup.Header, Is.EqualTo("Launchers"));

            Assert.That(secondGroup.Launchers, Is.Not.Null);
            Assert.That(secondGroup.Launchers, Has.Count.EqualTo(1));
            AssertSingleCommandLauncher("Rebellion", "[ROOT]/Rebels/Yavin/bin/debug/Start rebellion.cmd", secondGroup.Launchers[0]);

            Assert.That(secondGroup.SubGroups, Is.Not.Null);
            Assert.That(secondGroup.SubGroups, Has.Count.EqualTo(1));

            var subGroup = secondGroup.SubGroups[0];
            Assert.That(subGroup.Header, Is.EqualTo("Big ass launchers"));

            Assert.That(subGroup.Launchers, Has.Count.EqualTo(1));
            AssertSingleCommandLauncher("Jar-Jar you stink", "Kick Jar-Jar.ps1", subGroup.Launchers[0]);
        }

        private void AssertSingleCommandLauncher(string header, string command, XmlLauncherLink launcher)
        {
            Assert.That(launcher, Is.Not.Null);
            Assert.That(launcher.Header, Is.EqualTo(header));
            Assert.That(launcher.Commands, Has.Count.EqualTo(1));
            Assert.That(((XmlExecuteCommand)launcher.Commands[0]).Command, Is.EqualTo(command));
        }

        [Test]
        public void ShouldThrowWhenFilePathIsUnknown()
        {
            var service = new XmlLauncherConfigurationReader();
            Assert.That(() => service.LoadFile("E:/unknown_path"), Throws.Exception.TypeOf<InvalidOperationException>().And.Message.EqualTo("Unknow file path [E:/unknown_path]"));
        }
    }
}
