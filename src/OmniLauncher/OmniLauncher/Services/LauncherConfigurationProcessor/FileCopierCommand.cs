namespace OmniLauncher.Services.LauncherConfigurationProcessor
{
    public class FileCopierCommand : LauncherCommand
    {
        public string SourceFilePath { get; set; }

        public string TargetFilePath { get; set; }
    }
}