namespace PatchManager.Models
{
    public class Patch
    {
        public string Owner { get; set; }
        public Gerrit Gerrit { get; set; }
        public Jira Jira { get; set; }
        public PatchStatus Status { get; set; }
        public RiskOneAsset Asset { get; set; }

        public Patch()
        {
            Status = new PatchStatus();
        }
    }

    public class Gerrit
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}