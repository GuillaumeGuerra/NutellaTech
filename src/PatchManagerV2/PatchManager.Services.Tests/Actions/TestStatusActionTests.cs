using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PatchManager.Models;
using PatchManager.Services.PatchActions;
using PatchManager.TestFramework;
using PatchManager.TestFramework.Reflection;
using TestStatus = PatchManager.Models.TestStatus;

namespace PatchManager.Services.Tests.Actions
{
    [TestFixture]
    public class TestStatusActionTests
    {
        [TestCase(typeof(PatchTestedAction), TestStatus.Tested)]
        [TestCase(typeof(PatchToTestAction), TestStatus.ToTest)]
        [TestCase(typeof(IssueFoundDuringTestAction), TestStatus.Issue)]
        public void ShouldChangeTestStatusAndReturnTrueWhenPatchStatusIsDifferent(Type actionType, TestStatus status)
        {
            var action = (TestStatusAction)Activator.CreateInstance(actionType);
            Patch actualPatch = new Patch()
            {
                Status = new PatchStatus()
                {
                    // We make sure the patch had a different value
                    Test = EnumHelper<TestStatus>.GetOtherEnumValue(status)
                }
            };

            //It should return true, since the existing value of the RegistrationStatus was different from the existing one
            Assert.That(action.Apply(actualPatch), Is.True);
            Assert.That(actualPatch.Status.Test, Is.EqualTo(status));
        }

        [TestCase(typeof(PatchTestedAction), TestStatus.Tested)]
        [TestCase(typeof(PatchToTestAction), TestStatus.ToTest)]
        [TestCase(typeof(IssueFoundDuringTestAction), TestStatus.Issue)]
        public void ShouldLeaveTestStatusAndReturnFalseWhenPatchStatusIsSame(Type actionType, TestStatus status)
        {
            var action = (TestStatusAction)Activator.CreateInstance(actionType);
            Patch actualPatch = new Patch()
            {
                Status = new PatchStatus()
                {
                    // The patch already had the same value
                    Test = status
                }
            };

            //It should return false, since there was nothing to do to the path
            Assert.That(action.Apply(actualPatch), Is.False);
            Assert.That(actualPatch.Status.Test, Is.EqualTo(status));
        }
    }
}
