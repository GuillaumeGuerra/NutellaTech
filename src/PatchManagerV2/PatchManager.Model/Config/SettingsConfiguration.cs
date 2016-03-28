namespace PatchManager.Config
{
    public class SettingsConfiguration
    {
        public int TimeoutInMinutesToResolveGerritStatus { get; set; }
        public string JiraUrl { get; set; }
        public string GerritUrl { get; set; }
        public string PersistenceDirectoryPath { get; set; }
    }
}
