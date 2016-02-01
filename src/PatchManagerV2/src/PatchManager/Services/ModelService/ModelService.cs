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

        public List<Gerrit> GetPatchGerrits(string patchVersion)
        {
            // Copied to a list, to prevent modifications during iteration
            return TryGetPatch(patchVersion).Gerrits.Select(pair => pair.Value).ToList();
        }

        public Gerrit GetGerritForPatch(string patchVersion, int gerritId)
        {
            Gerrit gerrit;
            TryGetPatch(patchVersion).Gerrits.TryGetValue(gerritId, out gerrit);
            return gerrit;
        }

        public void AddGerritToPatch(string patchVersion, Gerrit gerrit)
        {
            var patch = TryGetPatch(patchVersion);
            if (patch.Patch == null)
                return; // Case of a non existing patch, probably an issue ...

            patch.Gerrits.Add(gerrit.Id, gerrit);
            Persistence.AddGerritToPatch(patch.Patch, gerrit);
        }

        private PatchWithGerrits TryGetPatch(string patchVersion)
        {
            PatchWithGerrits patch;

            if (_patchesDico.TryGetValue(patchVersion, out patch))
                return patch;

            return new PatchWithGerrits();
        }

        private void Initialize()
        {
            _patchesDico = new Dictionary<string, PatchWithGerrits>();
            foreach (var patch in Persistence.GetAllPatches())
            {
                _patchesDico.Add(patch.Version, new PatchWithGerrits(patch, Persistence.GetGerrits(patch.Version).ToDictionary(gerrit => gerrit.Id)));
            }
        }

        private class PatchWithGerrits
        {
            public Patch Patch { get; }
            public Dictionary<int, Gerrit> Gerrits { get; }

            public PatchWithGerrits(Patch patch, Dictionary<int, Gerrit> gerrits)
            {
                Patch = patch;
                Gerrits = gerrits;
            }

            public PatchWithGerrits()
            {
                Gerrits = new Dictionary<int, Gerrit>();
            }
        }
    }
}
