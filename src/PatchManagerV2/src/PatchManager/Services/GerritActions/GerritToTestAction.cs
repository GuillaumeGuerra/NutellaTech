using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [GerritAction("toTest")]
    public class GerritToTestAction : TestStatusAction
    {
        public GerritToTestAction() : base(TestStatus.ToTest) { }
    }
}