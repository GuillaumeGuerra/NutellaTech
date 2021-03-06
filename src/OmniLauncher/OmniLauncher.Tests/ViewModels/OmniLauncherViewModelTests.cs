﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using OmniLauncher.ViewModels;

namespace OmniLauncher.Tests.ViewModels
{
    [TestFixture]
    public class OmniLauncherViewModelTests
    {
        [Test]
        [Apartment(ApartmentState.STA)]
        [Ignore("Work in progress")]
        public void ShouldInitializeLaunchersWhenLoaded()
        {
            var vm = new OmniLauncherViewModel();

            // As long as the load is not completed, the VM should maintain the view closed
            Assert.That(vm.IsOpened, Is.False);

            Assert.That(vm.Launchers, Has.Count.EqualTo(0));

            vm.LoadedCommand.Execute(null);

            Thread.Sleep(5000);

            Assert.That(vm.Launchers, Has.Count.GreaterThan(0));

            // Now it should be opened
            Assert.That(vm.IsOpened, Is.True);
        }
    }
}
