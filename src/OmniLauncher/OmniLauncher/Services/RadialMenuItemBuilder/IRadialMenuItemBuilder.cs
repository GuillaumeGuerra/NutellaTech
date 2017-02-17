using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infragistics.Controls.Menus;
using OmniLauncher.Services.ConfigurationLoader;

namespace OmniLauncher.Services.RadialMenuItemBuilder
{
    public interface IRadialMenuItemBuilder
    {
        IEnumerable<RadialMenuItem> BuildMenuItems(LaunchersNode launchers);
    }
}