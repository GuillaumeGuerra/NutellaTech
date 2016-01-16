using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OmniLauncher.Services;
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
            XmlLauncherConfiguration actual = service.LoadFile("Data/LaunchersTest1.xml");
            Assert.That(actual.RootDirectories, Is.Not.Null);
            Assert.That(actual.RootDirectories.Count, Is.EqualTo(2));

            Assert.That(actual.RootDirectories[0].Path, Is.EqualTo("D:/Jar-Jar You Stink"));
            Assert.That(actual.RootDirectories[0].Header, Is.EqualTo("Jar-Jar"));
            Assert.That(actual.RootDirectories[1].Path, Is.EqualTo("E:/Star Trek/Kirk_you_suck"));
            Assert.That(actual.RootDirectories[1].Header, Is.EqualTo("Kirk"));

            Assert.That(actual.GenericTemplate, Is.Not.Null);
            Assert.That(actual.GenericTemplate.LinkGroups, Is.Not.Null);

            Assert.That(actual.GenericTemplate.LinkGroups.Count, Is.EqualTo(2));

            var firstGroup = actual.GenericTemplate.LinkGroups[0];
            Assert.That(firstGroup.Header, Is.EqualTo("Solutions"));
            Assert.That(firstGroup.Launchers, Is.Not.Null);

            Assert.That(firstGroup.Launchers[0], Is.Not.Null);
            Assert.That(firstGroup.Launchers[0].Header, Is.EqualTo("Base.sln"));
            Assert.That(firstGroup.Launchers[0].Command, Is.EqualTo("[ROOT]/Rebels/Yavin/base.sln"));

            Assert.That(firstGroup.Launchers[1], Is.Not.Null);
            Assert.That(firstGroup.Launchers[1].Header, Is.EqualTo("Padawan.sln"));
            Assert.That(firstGroup.Launchers[1].Command, Is.EqualTo("[ROOT]/Jedis/padawan.sln"));

            Assert.That(firstGroup.Launchers[2], Is.Not.Null);
            Assert.That(firstGroup.Launchers[2].Header, Is.EqualTo("Stormtrooper.sln"));
            Assert.That(firstGroup.Launchers[2].Command, Is.EqualTo("C:/Empire/stormtrooper.sln"));

            var secondGroup = actual.GenericTemplate.LinkGroups[1];
            Assert.That(secondGroup.Header, Is.EqualTo("Launchers"));
            Assert.That(secondGroup.Launchers, Is.Not.Null);

            Assert.That(secondGroup.Launchers[0], Is.Not.Null);
            Assert.That(secondGroup.Launchers[0].Header, Is.EqualTo("Rebellion"));
            Assert.That(secondGroup.Launchers[0].Command, Is.EqualTo("[ROOT]/Rebels/Yavin/bin/debug/Start rebellion.cmd"));
        }

        [Test]
        public void ShouldThrowWhenFilePathIsUnknown()
        {
            var service = new XmlLauncherConfigurationReader();
            Assert.That(() => service.LoadFile("E:/unknown_path"), Throws.Exception.TypeOf<InvalidOperationException>().And.Message.EqualTo("Unknow file path [E:/unknown_path]"));
        }
    }
}
