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

        public List<Patch> GetAllPatches()
        {
            // Copied to a list, to prevent modifications during iteration
            return _patchesDico.Select(pair => pair.Value.Patch).ToList();
        }

        public Patch GetPatch(string patchVersion)
        {
            return TryGetPatch(patchVersion).Patch;
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

        public void AddGerritToPatch(string patchVersion, Gerrit gerrit)
        {
            var patch = TryGetPatch(patchVersion);
            if (patch.Patch == null)
                return; // Case of a non existing patch, probably an issue ...

            patch.Gerrits.Add(gerrit.Id, new GerritWithMetadata(gerrit));
            Persistence.AddGerritToPatch(patch.Patch, gerrit);
        }

        public void UpdatePatchGerrit(string patchVersion, Gerrit gerrit)
        {
            PatchWithGerrits patch;
            var foundGerrit = TryGetPatchGerrit(patchVersion, gerrit.Id, out patch);
            if (foundGerrit == null)
                return; // Case of a non existing patch, probably an issue ...

            Persistence.UpdatePatchGerrit(patch.Patch, gerrit);
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
                _patchesDico.Add(patch.Version, new PatchWithGerrits(patch, Persistence.GetGerrits(patch.Version).Select(gerrit => new GerritWithMetadata(gerrit)).ToDictionary(gerrit => gerrit.Gerrit.Id)));
            }
        }

        private class PatchWithGerrits
        {
            public Patch Patch { get; }
            public Dictionary<int, GerritWithMetadata> Gerrits { get; }

            public PatchWithGerrits(Patch patch, Dictionary<int, GerritWithMetadata> gerrits)
            {
                Patch = patch;
                Gerrits = gerrits;
            }

            public PatchWithGerrits()
            {
                Gerrits = new Dictionary<int, GerritWithMetadata>();
            }
        }
    }
}
