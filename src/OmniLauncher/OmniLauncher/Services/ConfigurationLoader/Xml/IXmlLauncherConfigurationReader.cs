namespace OmniLauncher.Services.ConfigurationLoader.Xml
{
    public interface IXmlLauncherConfigurationReader
    {
        XmlLauncherConfiguration LoadFile(string filePath);
    }
}