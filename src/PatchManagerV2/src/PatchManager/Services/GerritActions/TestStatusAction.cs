using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    public abstract class TestStatusAction : IPatchAction
    {
        public TestStatus TestStatus { get; set; }

        public TestStatusAction(TestStatus testStatus)
        {
            TestStatus = testStatus;
        }

        public bool Apply(Patch patch)
        {
            if (patch.Status.Test == TestStatus)
                return false;

            patch.Status.Test = TestStatus;
            return true;
        }
    }
}