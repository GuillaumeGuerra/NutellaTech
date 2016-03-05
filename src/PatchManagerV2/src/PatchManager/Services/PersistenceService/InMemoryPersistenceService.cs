using System.Collections.Generic;
using PatchManager.Models;

namespace PatchManager.Services.PersistenceService
{
    /// <summary>
    /// This implementation doesn't do anything : everytime the website is restarted, we start from scratch again
    /// </summary>
    internal class InMemoryPersistenceService : IPersistenceService
    {
        public IEnumerable<Release> GetAllPatches()
        {
            return new List<Release>();
        }

        public IEnumerable<Patch> GetGerrits(string patchVersion)
        {
            return new List<Patch>();
        }

        public void AddGerritToPatch(Release release, Patch patch) { }

        public void UpdatePatchGerrit(Release release, Patch patch) { }
    }
}