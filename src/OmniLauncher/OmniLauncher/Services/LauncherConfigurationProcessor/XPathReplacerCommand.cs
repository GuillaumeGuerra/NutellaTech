namespace OmniLauncher.Services.LauncherConfigurationProcessor
{
    public class XPathReplacerCommand : LauncherCommand
    {
        public string FilePath { get; set; }

        public string XPath { get; set; }
        
        public string Value { get; set; }
    }
}