using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    public abstract class PatchStatusAction : IGerritAction
    {
        public PatchStatus PatchStatus { get; set; }

        public PatchStatusAction(PatchStatus patchStatus)
        {
            PatchStatus = patchStatus;
        }

        public bool Apply(Gerrit gerrit)
        {
            if (gerrit.Status.Patch == PatchStatus)
                return false;

            gerrit.Status.Patch = PatchStatus;
            return true;
        }
    }
}