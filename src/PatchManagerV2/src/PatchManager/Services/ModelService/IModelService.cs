using System.Collections.Generic;
using PatchManager.Models;

namespace PatchManager.Services.ModelService
{
    public interface IModelService
    {
        List<Release> GetAllReleases();

        Release GetRelease(string releaseVersion);

        List<PatchWithMetadata> GetReleasePatches(string releaseVersion);

        PatchWithMetadata GetReleasePatch(string releaseVersion, int patchId);

        void AddPatchToRelease(string releaseVersion, Patch patch);

        void UpdateReleasePatch(string releaseVersion, Patch patch);
    }
}