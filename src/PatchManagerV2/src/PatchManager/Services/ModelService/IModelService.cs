using System;
using System.Collections.Generic;
using PatchManager.Models;

namespace PatchManager.Services.ModelService
{
    public interface IModelService
    {
        List<Patch> GetAllPatches();

        Patch GetPatch(string patchVersion);

        List<GerritWithMetadata> GetPatchGerrits(string patchVersion);

        GerritWithMetadata GetGerritForPatch(string patchVersion, int gerritId);

        void AddGerritToPatch(string patchVersion, Gerrit gerrit);
    }
}