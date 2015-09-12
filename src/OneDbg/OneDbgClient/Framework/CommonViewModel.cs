using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;

namespace OneDbgClient.Framework
{
    public class CommonViewModel : ViewModelBase
    {
        protected IPopupService PopupService { get; set; }
        protected IThemeService ThemeService { get; set; }

        public CommonViewModel()
        {
            PopupService = ServiceLocator.Current.GetInstance<IPopupService>();
            ThemeService = ServiceLocator.Current.GetInstance<IThemeService>();
        }
    }
}
