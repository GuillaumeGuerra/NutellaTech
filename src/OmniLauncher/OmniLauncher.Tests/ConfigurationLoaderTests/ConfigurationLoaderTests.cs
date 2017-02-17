﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OmniLauncher.Services.CommandLauncher;
using OmniLauncher.Tests.Framework;
using OmniLauncher.Services.ConfigurationLoader;
using OmniLauncher.Services.LauncherService;

namespace OmniLauncher.Tests.ConfigurationLoaderTests
{
    [TestFixture]
    public class ConfigurationLoaderTests
    {
        private ContainerOverrider _overrider;

        [SetUp]
        public void SetUp()
        {
            _overrider = new ContainerOverrider();
        }

        [TearDown]
        public void TearDown()
        {
            _overrider.Dispose();
        }

        [Test]
        public void ShouldReadAllFilesLocatedInTheConfigurationDirectoryWithTheRightPlugin()
        {
            using (var directory = new TemporaryDirectory())
            {
                File.WriteAllText($"{directory.Location}\\file1.xml", "");
                File.WriteAllText($"{directory.Location}\\file2.json", "");

                // This one is not known by any plugin, it should be ignored
                File.WriteAllText($"{directory.Location}\\file3.bin", "");

                var firstNode = new LaunchersNode();
                var secondNode = new LaunchersNode();

                var xmlLoader = new Mock<ILauncherConfigurationProcessor>(MockBehavior.Strict);
                xmlLoader.Setup(mock => mock.CanProcess(It.IsAny<string>()))
                    .Returns<string>(s => s.EndsWith(".xml"))
                    .Verifiable();
                xmlLoader.Setup(mock => mock.Load($"{directory.Location}\\file1.xml")).Returns(firstNode).Verifiable();

                var jsonLoader = new Mock<ILauncherConfigurationProcessor>(MockBehavior.Strict);
                jsonLoader.Setup(mock => mock.CanProcess(It.IsAny<string>()))
                    .Returns<string>(s => s.EndsWith(".json"))
                    .Verifiable();
                jsonLoader.Setup(mock => mock.Load($"{directory.Location}\\file2.json")).Returns(secondNode).Verifiable();

                var builder = new ContainerBuilder();

                // NB : we register this plugin first, to ensure at least one plugin refuses all given commands
                builder.RegisterInstance(xmlLoader.Object).As<ILauncherConfigurationProcessor>();
                builder.RegisterInstance(jsonLoader.Object).As<ILauncherConfigurationProcessor>();

                App.Container = builder.Build();

                var launchers = new ConfigurationLoader().LoadConfiguration(directory.Location).ToList();

                Assert.That(launchers, Has.Count.EqualTo(2));
                Assert.That(launchers[0], Is.SameAs(firstNode));
                Assert.That(launchers[1], Is.SameAs(secondNode));

                xmlLoader.VerifyAll();
            }
        }
    }
}
