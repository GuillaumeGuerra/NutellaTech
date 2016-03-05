namespace PatchManager.Models
{
    public class Patch
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Owner { get; set; }
        public Jira Jira { get; set; }
        public GerritStatus Status { get; set; }
        public RiskOneAsset Asset { get; set; }

        public Patch()
        {
            Status = new GerritStatus();
        }
    }
}