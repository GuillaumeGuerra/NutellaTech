using PatchManager.Models;

namespace PatchManager.Services.PatchActions
{
    [PatchAction("tested")]
    public class PatchTestedAction : TestStatusAction
    {
        public PatchTestedAction() : base(TestStatus.Tested) { }
    }
}