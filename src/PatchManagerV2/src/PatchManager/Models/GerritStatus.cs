namespace PatchManager.Models
{
    public class GerritStatus
    {
        public PatchStatus PatchStatus { get; set; }
        public JiraStatus JiraStatus { get; set; }
        public MergeStatus MergeStatus { get; set; }
        public TestStatus TestStatus { get; set; }
    }
}