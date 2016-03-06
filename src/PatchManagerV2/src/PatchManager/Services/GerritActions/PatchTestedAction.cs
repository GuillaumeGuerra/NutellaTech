using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [PatchAction("tested")]
    public class PatchTestedAction : TestStatusAction
    {
        public PatchTestedAction() : base(TestStatus.Tested) { }
    }
}