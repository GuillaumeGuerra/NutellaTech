namespace PatchManager.Models
{
    public class PatchStatus
    {
        public RegistrationStatus Registration { get; set; }
        public JiraStatus Jira { get; set; }
        public GerritStatus Gerrit { get; set; }
        public TestStatus Test { get; set; }

        public PatchStatus()
        {
            Registration = RegistrationStatus.Unknown;
            Jira = JiraStatus.Unknown;
            Gerrit = GerritStatus.Unknown;
            Test = TestStatus.Unknown;
        }
    }
}