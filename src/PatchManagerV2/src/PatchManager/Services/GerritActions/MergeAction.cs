using PatchManager.Models;
using PatchManager.Services.GerritService;

namespace PatchManager.Services.GerritActions
{
    [GerritAction("merge")]
    public class MergeAction : IGerritAction
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

            if (Service.Merge(patch.Id))
                patch.Status.Merge = MergeStatus.Merged;

            return true;
        }
    }
}