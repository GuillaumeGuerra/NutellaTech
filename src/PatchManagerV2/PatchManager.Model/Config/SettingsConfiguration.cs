namespace PatchManager.Config
{
    public class SettingsConfiguration
    {
        public static SettingsConfiguration Settings { get; set; }

        public int TimeoutInMinutesToResolveGerritStatus { get; set; }

        public string JiraUrl { get; set; }

        public string GerritUrl { get; set; }
    }
}
