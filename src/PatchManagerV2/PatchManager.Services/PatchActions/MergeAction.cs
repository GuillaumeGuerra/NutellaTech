using PatchManager.Model.Services;
using PatchManager.Models;

namespace PatchManager.Services.PatchActions
{
    [PatchAction("merge")]
    public class MergeAction : IPatchAction
    {
        public IGerritService Service { get; set; }

        public MergeAction(IGerritService service)
        {
            Service = service;
        }

        public bool Apply(Patch patch)
        {
            if (patch.Status.Merge != MergeStatus.ReadyForMerge)
                return false;

            if (Service.Merge(patch.Gerrit.Id))
                patch.Status.Merge = MergeStatus.Merged;

            return true;
        }
    }
}