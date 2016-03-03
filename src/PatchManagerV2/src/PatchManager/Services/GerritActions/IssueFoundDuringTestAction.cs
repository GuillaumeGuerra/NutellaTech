using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [GerritAction("issueFound")]
    public class IssueFoundDuringTestAction : TestStatusAction
    {
        public IssueFoundDuringTestAction() : base(TestStatus.Issue) { }
    }
}