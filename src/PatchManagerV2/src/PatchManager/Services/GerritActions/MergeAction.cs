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

        public bool Apply(Gerrit gerrit)
        {
            if (gerrit.Status.Merge != MergeStatus.ReadyForMerge)
                return false;

            if (Service.Merge(gerrit.Id))
                gerrit.Status.Merge = MergeStatus.Merged;

            return true;
        }
    }
}