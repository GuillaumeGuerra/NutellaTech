using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    public interface IBinaryProcessor
    {
        void ProcessBinaries(IList items, IObserver observer);
    }
}
