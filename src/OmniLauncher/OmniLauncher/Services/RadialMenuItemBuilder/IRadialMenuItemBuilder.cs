using System.Collections.ObjectModel;
using Infragistics.Controls.Menus;
using OmniLauncher.Services.LauncherConfigurationProcessor;

namespace OmniLauncher.Services.RadialMenuItemBuilder
{
    public interface IRadialMenuItemBuilder
    {
        ObservableCollection<RadialMenuItem> BuildMenuItems(LaunchersNode launchers);
    }
}