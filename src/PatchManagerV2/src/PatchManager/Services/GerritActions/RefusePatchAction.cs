using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [PatchAction("refuse")]
    public class RefusePatchAction : PatchStatusAction
    {
        public RefusePatchAction() : base(PatchStatus.Refused) { }
    }
}