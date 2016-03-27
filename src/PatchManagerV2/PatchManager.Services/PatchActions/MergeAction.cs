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
            if (patch.Status.Gerrit != GerritStatus.ReadyForMerge)
                return false;

            if (Service.Merge(patch.Gerrit.Id))
                patch.Status.Gerrit = GerritStatus.Merged;
            else
            {
                // We don't know what happened here, so we'll move the status back to Unknown
                patch.Status.Gerrit = GerritStatus.Unknown;
            }

            return true;
        }
    }
}