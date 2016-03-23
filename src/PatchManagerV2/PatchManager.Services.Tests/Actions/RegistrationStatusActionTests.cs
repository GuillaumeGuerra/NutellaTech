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

namespace PatchManager.Services.Tests.Actions
{
    [TestFixture]
    public class RegistrationStatusActionTests
    {
        [TestCase(typeof(AcceptRegistrationAction), RegistrationStatus.Accepted)]
        [TestCase(typeof(AskedRegistrationAction), RegistrationStatus.Asked)]
        [TestCase(typeof(RefuseRegistrationAction), RegistrationStatus.Refused)]
        public void ShouldChangeRegistrationStatusAndReturnTrueWhenPatchStatusIsDifferent(Type actionType, RegistrationStatus status)
        {
            var action = (RegistrationStatusAction)Activator.CreateInstance(actionType);
            Patch actualPatch = new Patch()
            {
                Status = new PatchStatus()
                {
                    // We make sure the patch had a different value
                    Registration = EnumHelper<RegistrationStatus>.GetOtherEnumValue(status)
                }
            };

            //It should return true, since the existing value of the RegistrationStatus was different from the existing one
            Assert.That(action.Apply(actualPatch), Is.True);
            Assert.That(actualPatch.Status.Registration, Is.EqualTo(status));
        }

        [TestCase(typeof(AcceptRegistrationAction), RegistrationStatus.Accepted)]
        [TestCase(typeof(AskedRegistrationAction), RegistrationStatus.Asked)]
        [TestCase(typeof(RefuseRegistrationAction), RegistrationStatus.Refused)]
        public void ShouldLeaveRegistrationStatusAndReturnFalseWhenPatchStatusIsSame(Type actionType, RegistrationStatus status)
        {
            RegistrationStatusAction action = (RegistrationStatusAction)Activator.CreateInstance(actionType);
            Patch actualPatch = new Patch()
            {
                Status = new PatchStatus()
                {
                    // The patch already had the same value
                    Registration = status
                }
            };

            //It should return false, since there was nothing to do to the path
            Assert.That(action.Apply(actualPatch), Is.False);
        }
    }
}
