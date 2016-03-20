namespace PatchManager.Models
{
    public class PatchStatus
    {
        public RegistrationStatus Registration { get; set; }
        public JiraStatus Jira { get; set; }
        public MergeStatus Merge { get; set; }
        public TestStatus Test { get; set; }

        public PatchStatus()
        {
            Registration = RegistrationStatus.Unknown;
            Jira = JiraStatus.Unknown;
            Merge = MergeStatus.Unknown;
            Test = TestStatus.Unknown;
        }
    }
}