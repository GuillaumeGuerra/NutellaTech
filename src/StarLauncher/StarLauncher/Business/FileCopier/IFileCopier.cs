using StarLauncher.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    public interface IFileCopier
    {
        void CopyFiles(StarEnvironment environment, IObserver observer);
    }
}
