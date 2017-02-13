using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using OmniLauncher.Services.IExceptionManager;
using OmniLauncher.Services.LauncherConfigurationProcessor;
using OmniLauncher.Services.LauncherService;

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
                .Setup(mock => mock.ShowException(It.IsAny<Win32Exception>()))
                .Verifiable();

            var launcherService = new LauncherService() { MessageService = message.Object };
            launcherService.Launch(new LauncherLink() { Command = "What/Is/This/Fucking/Command" });

            message.VerifyAll();
        }

        [Test]
        public void ShouldLaunchConsoleAppWhenCommandIsValid()
        {
            // Strict, and no expectation, since the service shouldn't be called as no exception is expected
            var message = new Mock<IMessageService>(MockBehavior.Strict);

            var launcherService = new LauncherService() { MessageService = message.Object };

            // So far, no process should be running
            Assert.That(Process.GetProcessesByName("TestConsoleApplication"), Has.Length.EqualTo(0));

            launcherService.Launch(new LauncherLink() { Command = "TestConsoleApplication.exe" });

            // Wait for a few milliseconds, to ensure the process had time to start
            Thread.Sleep(1000);

            var processes = Process.GetProcessesByName("TestConsoleApplication");
            Assert.That(processes, Has.Length.EqualTo(1));

            // Now we try to kill the process, no need to keep it alive
            try
            {
                processes.First().Kill();
            }
            catch (Exception e)
            {
                // The process may have commited suicide on its own
            }

            message.VerifyAll();
        }
    }
}