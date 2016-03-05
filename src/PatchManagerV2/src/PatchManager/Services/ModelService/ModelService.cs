using System.Collections.Generic;
using System.Linq;
using PatchManager.Models;
using PatchManager.Services.PersistenceService;

namespace PatchManager.Services.ModelService
{
    public class ModelService : IModelService
    {
        public IPersistenceService Persistence { get; set; }
        private Dictionary<string, PatchWithGerrits> _patchesDico;

        public ModelService(IPersistenceService persistence)
        {
            Persistence = persistence;
            Initialize();
        }

        public List<Release> GetAllPatches()
        {
            // Copied to a list, to prevent modifications during iteration
            return _patchesDico.Select(pair => pair.Value.Release).ToList();
        }

        public Release GetPatch(string patchVersion)
        {
            return TryGetPatch(patchVersion).Release;
        }

        public List<GerritWithMetadata> GetPatchGerrits(string patchVersion)
        {
            // Copied to a list, to prevent modifications during iteration
            return TryGetPatch(patchVersion).Gerrits.Select(pair => pair.Value).ToList();
        }

        public GerritWithMetadata GetGerritForPatch(string patchVersion, int gerritId)
        {
            GerritWithMetadata gerrit;
            TryGetPatch(patchVersion).Gerrits.TryGetValue(gerritId, out gerrit);
            return gerrit;
        }

        public void AddGerritToPatch(string patchVersion, Patch patch)
        {
            var release = TryGetPatch(patchVersion);
            if (release.Release == null)
                return; // Case of a non existing patch, probably an issue ...

            release.Gerrits.Add(patch.Id, new GerritWithMetadata(patch));
            Persistence.AddGerritToPatch(release.Release, patch);
        }

        public void UpdatePatchGerrit(string patchVersion, Patch patch)
        {
            PatchWithGerrits release;
            var foundGerrit = TryGetPatchGerrit(patchVersion, patch.Id, out release);
            if (foundGerrit == null)
                return; // Case of a non existing patch, probably an issue ...

            Persistence.UpdatePatchGerrit(release.Release, patch);
        }

        private PatchWithGerrits TryGetPatch(string patchVersion)
        {
            PatchWithGerrits patch;

            if (_patchesDico.TryGetValue(patchVersion, out patch))
                return patch;

            return new PatchWithGerrits();
        }

        private GerritWithMetadata TryGetPatchGerrit(string patchVersion, int gerritId, out PatchWithGerrits patch)
        {
            patch = TryGetPatch(patchVersion);
            if (patch == null)
                return null;

            GerritWithMetadata gerrit;
            if (patch.Gerrits.TryGetValue(gerritId, out gerrit))
                return gerrit;

            return null;
        }

        private void Initialize()
        {
            _patchesDico = new Dictionary<string, PatchWithGerrits>();
            foreach (var patch in Persistence.GetAllPatches())
            {
                _patchesDico.Add(patch.Version, new PatchWithGerrits(patch, Persistence.GetGerrits(patch.Version).Select(gerrit => new GerritWithMetadata(gerrit)).ToDictionary(gerrit => gerrit.Patch.Id)));
            }
        }

        private class PatchWithGerrits
        {
            public Release Release { get; }
            public Dictionary<int, GerritWithMetadata> Gerrits { get; }

            public PatchWithGerrits(Release release, Dictionary<int, GerritWithMetadata> gerrits)
            {
                Release = release;
                Gerrits = gerrits;
            }

            public PatchWithGerrits()
            {
                Gerrits = new Dictionary<int, GerritWithMetadata>();
            }
        }
    }
}
