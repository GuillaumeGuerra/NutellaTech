using StarLauncher.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    public interface IEnvironmentLauncher
    {
        void LaunchEnvironment(StarEnvironment environment, IObserver observer);
    }
}
