using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using OmniLauncher.Services.CommandLauncher;
using OmniLauncher.Services.LauncherService;
using OmniLauncher.Services.MessageService;

namespace OmniLauncher.Tests.CommandLauncher
{
    [TestFixture]
    public class ExecuteCommandLauncherTests : CommonCommandLauncherTests<ExecuteCommandLauncher, ExecuteCommand>
    {
        [Test]
        public void ShouldLaunchConsoleAppWhenCommandIsValid()
        {
            // So far, no process should be running
            Assert.That(Process.GetProcessesByName("TestConsoleApplication"), Has.Length.EqualTo(0));

            new ExecuteCommandLauncher().Execute(new ExecuteCommand() { Command = "TestConsoleApplication.exe" });

            // Wait for a few milliseconds, to ensure the process had time to start
            Thread.Sleep(1000);

            var processes = Process.GetProcessesByName("TestConsoleApplication");
            Assert.That(processes, Has.Length.EqualTo(1));

            // Now we try to kill the process, no need to keep it alive
            try
            {
                processes.First().Kill();
            }
            catch (Exception)
            {
                // The process may have commited suicide on its own
            }
        }
    }
}