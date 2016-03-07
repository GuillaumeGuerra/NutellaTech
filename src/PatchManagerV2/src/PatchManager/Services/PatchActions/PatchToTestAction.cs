using PatchManager.Models;

namespace PatchManager.Services.PatchActions
{
    [PatchAction("toTest")]
    public class PatchToTestAction : TestStatusAction
    {
        public PatchToTestAction() : base(TestStatus.ToTest) { }
    }
}