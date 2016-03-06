using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [PatchAction("ask")]
    public class AskedPatchAction : PatchStatusAction
    {
        public AskedPatchAction() : base(PatchStatus.Asked) { }
    }
}