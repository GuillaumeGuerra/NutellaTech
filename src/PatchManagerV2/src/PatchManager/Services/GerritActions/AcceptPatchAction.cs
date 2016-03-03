using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [GerritAction("accept")]
    public class AcceptPatchAction : PatchStatusAction
    {
        public AcceptPatchAction() : base(PatchStatus.Accepted) { }
    }
}