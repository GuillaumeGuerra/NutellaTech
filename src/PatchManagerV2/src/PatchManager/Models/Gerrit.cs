namespace PatchManager.Models
{
    public class Gerrit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public Jira Jira { get; set; }
        public GerritStatus Status { get; set; }

        public Gerrit()
        {
            Status = new GerritStatus();
        }
    }
}