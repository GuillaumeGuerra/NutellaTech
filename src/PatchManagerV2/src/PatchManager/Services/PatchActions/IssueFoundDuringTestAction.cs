using PatchManager.Models;

namespace PatchManager.Services.PatchActions
{
    [PatchAction("issueFound")]
    public class IssueFoundDuringTestAction : TestStatusAction
    {
        public IssueFoundDuringTestAction() : base(TestStatus.Issue) { }
    }
}