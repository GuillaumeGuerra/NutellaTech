using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [PatchAction("accept")]
    public class AcceptPatchAction : PatchStatusAction
    {
        public AcceptPatchAction() : base(PatchStatus.Accepted) { }
    }
}