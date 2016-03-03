using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [GerritAction("ask")]
    public class AskedPatchAction : PatchStatusAction
    {
        public AskedPatchAction() : base(PatchStatus.Asked) { }
    }
}