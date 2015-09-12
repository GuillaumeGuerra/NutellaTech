using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using Infragistics.Windows.Themes;

namespace OneDbgClient.Framework
{
    public class ThemeService : IThemeService
    {
        private List<FrameworkElement> ThemedElements { get; set; }
        private string ActiveGridTheme { get; set; }

        public ThemeService()
        {
            ThemedElements = new List<FrameworkElement>();
            ActiveGridTheme = "Office2010Blue";
        }

        public void ApplyTheme(string gridTheme)
        {
            ActiveGridTheme = gridTheme;
            ThemedElements.ForEach(element => ThemeManager.SetTheme(element, ActiveGridTheme));
        }

        public void RegisterThemedObject(FrameworkElement element)
        {
            ThemedElements.Add(element);
            ThemeManager.SetTheme(element, ActiveGridTheme);
        }

        public void UnregisterThemedObject(FrameworkElement element)
        {
            ThemedElements.Remove(element);
        }
    }
}