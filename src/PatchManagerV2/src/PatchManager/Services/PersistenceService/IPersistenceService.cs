using System.Collections.Generic;
using PatchManager.Models;

namespace PatchManager.Services.PersistenceService
{
    public interface IPersistenceService
    {
        IEnumerable<Release> GetAllPatches();
        IEnumerable<Patch> GetGerrits(string patchVersion);
        void AddGerritToPatch(Release release, Patch patch);
        void UpdatePatchGerrit(Release release, Patch patch);
    }
}