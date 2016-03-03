using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    public abstract class TestStatusAction : IGerritAction
    {
        public TestStatus TestStatus { get; set; }

        public TestStatusAction(TestStatus testStatus)
        {
            TestStatus = testStatus;
        }

        public bool Apply(Gerrit gerrit)
        {
            if (gerrit.Status.Test == TestStatus)
                return false;

            gerrit.Status.Test = TestStatus;
            return true;
        }
    }
}