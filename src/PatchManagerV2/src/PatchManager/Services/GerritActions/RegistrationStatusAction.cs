using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    public abstract class RegistrationStatusAction : IPatchAction
    {
        public RegistrationStatus RegistrationStatus { get; set; }

        public RegistrationStatusAction(RegistrationStatus registrationStatus)
        {
            RegistrationStatus = registrationStatus;
        }

        public bool Apply(Patch patch)
        {
            if (patch.Status.Registration == RegistrationStatus)
                return false;

            patch.Status.Registration = RegistrationStatus;
            return true;
        }
    }
}