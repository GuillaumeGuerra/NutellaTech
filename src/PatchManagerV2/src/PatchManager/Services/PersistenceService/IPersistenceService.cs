using System.Collections.Generic;
using PatchManager.Models;

namespace PatchManager.Services.PersistenceService
{
    public interface IPersistenceService
    {
        IEnumerable<Patch> GetAllPatches();
        IEnumerable<Gerrit> GetGerrits(string patchVersion);
        void AddGerritToPatch(Patch patch, Gerrit gerrit);
    }
}