﻿using System;
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
        [Test]
        public void ShouldCatchExceptionAndShowErrorWhenLaunchFails()
        {
            var message = new Mock<IMessageService>(MockBehavior.Strict);
            message
                .Setup(mock => mock.ShowException(It.Is<Exception>(e => e.Message == "Big badabig boom")))
                .Verifiable();

            var plugin = new Mock<ICommandLauncher>(MockBehavior.Strict);
            plugin.Setup(mock => mock.CanProcess(It.IsAny<LauncherCommand>())).Returns(true).Verifiable();
            plugin.Setup(mock => mock.Execute(It.IsAny<LauncherCommand>())).Throws(new Exception("Big badabig boom")).Verifiable();

            var launcherService = new LauncherService() { MessageService = message.Object, AllCommandLaunchers = new[] { plugin.Object } };
            launcherService.Launch(new LauncherLink() { Commands = new List<LauncherCommand>() { new ExecuteCommand() } });

            message.VerifyAll();
            plugin.VerifyAll();
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

            // NB : we register otherPluginMock first, to ensure at least one plugin refuses all given commands
            var launcher = new LauncherService() { AllCommandLaunchers = new[] { otherPluginMock.Object, executeMock.Object, xpathMock.Object } };
            launcher.Launch(new LauncherLink()
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
            var message = new Mock<IMessageService>(MockBehavior.Strict);
            message
                .Setup(mock => mock.ShowException(It.IsAny<NotSupportedException>()))
                .Verifiable();

            // No command launcher plugins at all, so obviously the service won't find any matching plugin
            var launcherService = new LauncherService() { MessageService = message.Object, AllCommandLaunchers = new ICommandLauncher[0] };
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

            var message = new Mock<IMessageService>(MockBehavior.Strict);
            message
                .Setup(mock => mock.ShowException(It.Is<Exception>(e => e.Message == "Big badabig boom"))) // The fifth element, leeloo I love you ...
                .Verifiable();

            var launcherService = new LauncherService() { MessageService = message.Object, AllCommandLaunchers = new[] { executeMock.Object } };
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