using StarLauncher.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    public interface IJumpListManager
    {
        void UpdateJumpList(List<StarEnvironment> environments);

        void UpdateRecentList(StarEnvironment environment);
    }
}
