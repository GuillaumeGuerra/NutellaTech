namespace PatchManager.Models
{
    public class Gerrit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Jira { get; set; }
        public GerritStatus Status { get; set; }
    }
}