using System;
using NUnit.Framework;
using OmniLauncher.Services.CommandLauncher;
using OmniLauncher.Services.ConfigurationLoader;

namespace OmniLauncher.Tests.CommandLauncher
{
    [TestFixture]
    public class BaseCommandLauncherTests
    {
        [Test]
        public void ShouldThrowWhenTypeOfCommandDoesNotMatch()
        {
            var expectedMessage =
                "Invalid command type [OmniLauncher.Services.ConfigurationLoader.XPathReplacerCommand] given to CommandLauncher plugin [OmniLauncher.Tests.CommandLauncher.BaseCommandLauncherTests+MockCommandLauncher]";

            Assert.That(() => new MockCommandLauncher().Execute(new XPathReplacerCommand()), Throws.Exception.TypeOf<NotSupportedException>().With.Message.EqualTo(expectedMessage));
        }

        private class MockCommandLauncher : BaseCommandLauncher<ExecuteCommand>
        {
            protected override void DoExecute(ExecuteCommand command)
            {
                // Should not be called !
                throw new NotImplementedException();
            }
        }
    }
}