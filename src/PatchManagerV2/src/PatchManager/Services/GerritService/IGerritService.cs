﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatchManager.Services.GerritService
{
    public interface IGerritService
    {
        GerritMetadata GetGerritMetadata(int gerritId);
    }
}
