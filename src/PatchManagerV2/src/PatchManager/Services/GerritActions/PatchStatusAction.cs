using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    public abstract class PatchStatusAction : IPatchAction
    {
        public PatchStatus PatchStatus { get; set; }

        public PatchStatusAction(PatchStatus patchStatus)
        {
            PatchStatus = patchStatus;
        }

        public bool Apply(Patch patch)
        {
            if (patch.Status.Patch == PatchStatus)
                return false;

            patch.Status.Patch = PatchStatus;
            return true;
        }
    }
}