﻿using System.Collections.Generic;
using System.Linq;
using PatchManager.Models;
using PatchManager.Services.PersistenceService;

namespace PatchManager.Services.ModelService
{
    public class ModelService : IModelService
    {
        public IPersistenceService Persistence { get; set; }
        private Dictionary<string, ReleaseWithPatches> ReleasesDico { get; set; }

        public ModelService(IPersistenceService persistence)
        {
            Persistence = persistence;
            Initialize();
        }

        public List<Release> GetAllReleases()
        {
            // Copied to a list, to prevent modifications during iteration
            return ReleasesDico.Select(pair => pair.Value.Release).ToList();
        }

        public Release GetRelease(string releaseVersion)
        {
            return TryGetRelease(releaseVersion).Release;
        }

        public List<PatchWithMetadata> GetReleasePatches(string releaseVersion)
        {
            // Copied to a list, to prevent modifications during iteration
            return TryGetRelease(releaseVersion).Patches.Select(pair => pair.Value).ToList();
        }

        public PatchWithMetadata GetReleasePatch(string releaseVersion, int gerritId)
        {
            PatchWithMetadata patch;
            TryGetRelease(releaseVersion).Patches.TryGetValue(gerritId, out patch);
            return patch;
        }

        public void AddPatchToRelease(string releaseVersion, Patch patch)
        {
            var release = TryGetRelease(releaseVersion);
            if (release.Release == null)
                return; // Case of a non existing patch, probably an issue ...

            release.Patches.Add(patch.Id, new PatchWithMetadata(patch));
            Persistence.AddPatchToRelease(release.Release, patch);
        }

        public void UpdateReleasePatch(string releaseVersion, Patch patch)
        {
            ReleaseWithPatches release;
            var foundPatch = TryGetReleasePatch(releaseVersion, patch.Id, out release);
            if (foundPatch == null)
                return; // Case of a non existing patch, probably an issue ...

            Persistence.UpdateReleasePatch(release.Release, patch);
        }

        private ReleaseWithPatches TryGetRelease(string releaseVersion)
        {
            ReleaseWithPatches release;

            if (ReleasesDico.TryGetValue(releaseVersion, out release))
                return release;

            return new ReleaseWithPatches();
        }

        private PatchWithMetadata TryGetReleasePatch(string patchVersion, int gerritId, out ReleaseWithPatches release)
        {
            release = TryGetRelease(patchVersion);
            if (release == null)
                return null;

            PatchWithMetadata patch;
            if (release.Patches.TryGetValue(gerritId, out patch))
                return patch;

            return null;
        }

        private void Initialize()
        {
            ReleasesDico = new Dictionary<string, ReleaseWithPatches>();
            foreach (var patch in Persistence.GetAllReleases())
            {
                ReleasesDico.Add(patch.Version, new ReleaseWithPatches(patch, Persistence.GetPatches(patch.Version).Select(gerrit => new PatchWithMetadata(gerrit)).ToDictionary(gerrit => gerrit.Patch.Id)));
            }
        }

        private class ReleaseWithPatches
        {
            public Release Release { get; }
            public Dictionary<int, PatchWithMetadata> Patches { get; }

            public ReleaseWithPatches(Release release, Dictionary<int, PatchWithMetadata> patches)
            {
                Release = release;
                Patches = patches;
            }

            public ReleaseWithPatches()
            {
                Patches = new Dictionary<int, PatchWithMetadata>();
            }
        }
    }
}
