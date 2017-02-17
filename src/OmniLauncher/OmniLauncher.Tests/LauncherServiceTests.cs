using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Autofac;
using Moq;
using NUnit.Framework;
using OmniLauncher.Framework;
using OmniLauncher.Services.CommandLauncher;
using OmniLauncher.Services.ConfigurationLoader;
using OmniLauncher.Services.LauncherService;
using OmniLauncher.Services.MessageService;
using OmniLauncher.Services.RadialMenuItemBuilder;
using OmniLauncher.Tests.CommandLauncher;
using OmniLauncher.Tests.Framework;
using OmniLauncher.ViewModels;
using IContainer = Autofac.IContainer;

namespace OmniLauncher.Tests
{
    [TestFixture]
    public class LauncherServiceTests
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
        public void ShouldCatchExceptionAndShowErrorWhenLaunchFails()
        {
            var message = new Mock<IMessageService>(MockBehavior.Strict);
            message
                .Setup(mock => mock.ShowException(It.IsAny<Win32Exception>()))
                .Verifiable();

            var launcherService = new LauncherService() { MessageService = message.Object };
            launcherService.Launch(new LauncherLink()
            {
                Commands = new List<LauncherCommand>()
                {
                    new ExecuteCommand() {Command = "What/Is/This/Fucking/Command"}
                }
            });

            message.VerifyAll();
        }

        [Test]
        public void ShouldFindPluginForEachCommandGivenInTheLauncher()
        {
            var executeCommand = new ExecuteCommand();
            var xpathCommand = new XPathReplacerCommand();

            var executeMock = new Mock<ICommandLauncher>(MockBehavior.Strict);
            executeMock.Setup(mock => mock.Execute(It.IsIn(executeCommand))).Verifiable();
            executeMock.Setup(mock => mock.CanProcess(It.IsAny<LauncherCommand>())).Returns<LauncherCommand>(c => ReferenceEquals(c, executeCommand)).Verifiable();

            var xpathMock = new Mock<ICommandLauncher>(MockBehavior.Strict);
            xpathMock.Setup(mock => mock.Execute(It.IsIn(xpathCommand))).Verifiable();
            xpathMock.Setup(mock => mock.CanProcess(It.IsAny<LauncherCommand>())).Returns<LauncherCommand>(c => ReferenceEquals(c, xpathCommand)).Verifiable();

            var otherPluginMock = new Mock<ICommandLauncher>(MockBehavior.Strict);
            otherPluginMock.Setup(mock => mock.CanProcess(It.IsAny<LauncherCommand>())).Returns(false).Verifiable();

            var builder = new ContainerBuilder();

            // NB : we register this plugin first, to ensure at least one plugin refuses all given commands
            builder.RegisterInstance(otherPluginMock.Object).As<ICommandLauncher>();
            builder.RegisterInstance(executeMock.Object).As<ICommandLauncher>();
            builder.RegisterInstance(xpathMock.Object).As<ICommandLauncher>();

            App.Container = builder.Build();

            new LauncherService().Launch(new LauncherLink()
            {
                Commands = new List<LauncherCommand>() { executeCommand, xpathCommand }
            });

            executeMock.VerifyAll();
            xpathMock.VerifyAll();
            otherPluginMock.VerifyAll();
        }

        [Test]
        public void ShouldRaiseProperExceptionWhenNoPluginCanBeFoundForAParticularCommand()
        {
            var builder = new ContainerBuilder();
            // No registration at all, so obviously the service won't find any matching plugin
            App.Container = builder.Build();

            var message = new Mock<IMessageService>(MockBehavior.Strict);
            message
                .Setup(mock => mock.ShowException(It.IsAny<NotSupportedException>()))
                .Verifiable();

            var launcherService = new LauncherService() { MessageService = message.Object };
            launcherService.Launch(new LauncherLink()
            {
                Commands = new List<LauncherCommand>()
                {
                    new ExecuteCommand()
                }
            });
        }

        [Test]
        public void ShouldRaiseProperExceptionWhenAPluginFailsToExecuteAParticularCommand()
        {
            var executeMock = new Mock<ICommandLauncher>(MockBehavior.Strict);
            executeMock.Setup(mock => mock.Execute(It.IsAny<LauncherCommand>())).Throws(new Exception("Big badabig boom")).Verifiable();
            executeMock.Setup(mock => mock.CanProcess(It.IsAny<LauncherCommand>())).Returns(true).Verifiable();

            var builder = new ContainerBuilder();

            builder.RegisterInstance(executeMock.Object).As<ICommandLauncher>();

            App.Container = builder.Build();

            var message = new Mock<IMessageService>(MockBehavior.Strict);
            message
                .Setup(mock => mock.ShowException(It.Is<Exception>(e => e.Message == "Big badabig boom"))) // The fifth element, leeloo I love you ...
                .Verifiable();

            var launcherService = new LauncherService() { MessageService = message.Object };
            launcherService.Launch(new LauncherLink()
            {
                Commands = new List<LauncherCommand>()
                {
                    new ExecuteCommand()
                }
            });
        }

        [Test]
        public void AllCommandLauncherPluginsShouldHaveADedicatedTestClass()
        {
            var testedTypes =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(
                        t =>
                            t.BaseType != null &&
                            t.BaseType.IsGenericType &&
                            t.BaseType.GetGenericTypeDefinition() == typeof(CommonCommandLauncherTests<,>))
                            .Select(t => t.BaseType.GetGenericArguments()[0])
                    .ToList();

            foreach (var plugin in App.Container.GetImplementations<ICommandLauncher>())
            {
                var pluginType = plugin.GetType();
                if (testedTypes.All(t => t != pluginType))
                    Assert.Fail($"Missing test class for ICommandLauncher type [{pluginType.FullName}]");
            }
        }
    }
}