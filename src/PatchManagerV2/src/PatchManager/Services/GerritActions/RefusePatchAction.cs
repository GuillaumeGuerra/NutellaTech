using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [GerritAction("refuse")]
    public class RefusePatchAction : PatchStatusAction
    {
        public RefusePatchAction() : base(PatchStatus.Refused) { }
    }
}