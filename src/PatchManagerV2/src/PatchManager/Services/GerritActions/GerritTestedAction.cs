using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [GerritAction("tested")]
    public class GerritTestedAction : TestStatusAction
    {
        public GerritTestedAction() : base(TestStatus.Tested) { }
    }
}