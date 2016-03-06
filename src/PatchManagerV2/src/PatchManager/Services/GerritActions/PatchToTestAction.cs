using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [PatchAction("toTest")]
    public class PatchToTestAction : TestStatusAction
    {
        public PatchToTestAction() : base(TestStatus.ToTest) { }
    }
}