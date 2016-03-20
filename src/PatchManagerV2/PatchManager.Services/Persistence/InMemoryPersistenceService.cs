using System.Collections.Generic;
using PatchManager.Model.Services;
using PatchManager.Models;

namespace PatchManager.Services.Persistence
{
    /// <summary>
    /// This implementation doesn't do anything : everytime the website is restarted, we start from scratch again
    /// </summary>
    internal class InMemoryPersistenceService : IPersistenceService
    {
        public IEnumerable<Release> GetAllReleases()
        {
            return new List<Release>();
        }

        public IEnumerable<Patch> GetPatches(string releaseVersion)
        {
            return new List<Patch>();
        }

        public void AddPatchToRelease(Release release, Patch patch) { }

        public void UpdateReleasePatch(Release release, Patch patch) { }
    }
}