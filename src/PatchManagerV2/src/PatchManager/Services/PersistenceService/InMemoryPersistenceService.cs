using System.Collections.Generic;
using PatchManager.Models;

namespace PatchManager.Services.PersistenceService
{
    /// <summary>
    /// This implementation doesn't do anything : everytime the website is restarted, we start from scratch again
    /// </summary>
    internal class InMemoryPersistenceService : IPersistenceService
    {
        public IEnumerable<Patch> GetAllPatches()
        {
            return new List<Patch>();
        }

        public IEnumerable<Gerrit> GetGerrits(string patchVersion)
        {
            return new List<Gerrit>();
        }

        public void AddGerritToPatch(Patch patch, Gerrit gerrit) { }

        public void UpdatePathGerrit(Patch patch, Gerrit gerrit) { }
    }
}