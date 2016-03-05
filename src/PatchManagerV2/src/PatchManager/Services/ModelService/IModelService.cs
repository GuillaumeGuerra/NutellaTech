using System.Collections.Generic;
using PatchManager.Models;

namespace PatchManager.Services.ModelService
{
    public interface IModelService
    {
        List<Release> GetAllPatches();

        Release GetPatch(string patchVersion);

        List<GerritWithMetadata> GetPatchGerrits(string patchVersion);

        GerritWithMetadata GetGerritForPatch(string patchVersion, int gerritId);

        void AddGerritToPatch(string patchVersion, Patch patch);

        void UpdatePatchGerrit(string patchVersion, Patch patch);
    }
}