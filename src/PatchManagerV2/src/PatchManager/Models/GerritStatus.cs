namespace PatchManager.Models
{
    public class GerritStatus
    {
        public PatchStatus Patch { get; set; }
        public JiraStatus Jira { get; set; }
        public MergeStatus Merge { get; set; }
        public TestStatus Test { get; set; }
    }
}