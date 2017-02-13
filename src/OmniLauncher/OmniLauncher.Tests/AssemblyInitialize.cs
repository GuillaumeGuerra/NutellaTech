using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
