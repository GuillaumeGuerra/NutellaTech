using System;
using NUnit.Framework;

namespace OmniLauncher.Tests
{
    [SetUpFixture]
    public class AssemblyInitialize
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            App.ConfigureDependencyInjection();
        }
    }
}
