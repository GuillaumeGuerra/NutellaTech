﻿using System.Collections.Generic;
using PatchManager.Models;

namespace PatchManager.Model.Services
{
    public interface IPersistenceService
    {
        IEnumerable<Release> GetAllReleases();
        IEnumerable<Patch> GetPatches(string releaseVersion);
        void AddPatchToRelease(Release release, Patch patch);
        void UpdateReleasePatch(Release release, Patch patch);
    }
}