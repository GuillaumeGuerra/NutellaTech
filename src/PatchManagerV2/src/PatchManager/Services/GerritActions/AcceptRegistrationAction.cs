using PatchManager.Models;

namespace PatchManager.Services.GerritActions
{
    [PatchAction("accept")]
    public class AcceptRegistrationAction : RegistrationStatusAction
    {
        public AcceptRegistrationAction() : base(RegistrationStatus.Accepted) { }
    }
}