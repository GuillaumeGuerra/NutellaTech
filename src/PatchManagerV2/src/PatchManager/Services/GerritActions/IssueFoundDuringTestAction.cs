using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [PatchAction("issueFound")]
    public class IssueFoundDuringTestAction : TestStatusAction
    {
        public IssueFoundDuringTestAction() : base(TestStatus.Issue) { }
    }
}