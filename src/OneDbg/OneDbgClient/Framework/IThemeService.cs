using System.Windows;
using Infragistics.Windows.DataPresenter;

namespace OneDbgClient.Framework
{
    public interface IThemeService
    {
        void ApplyTheme(string gridTheme);
        void RegisterThemedObject(FrameworkElement element);
        void UnregisterThemedObject(FrameworkElement element);
    }
}