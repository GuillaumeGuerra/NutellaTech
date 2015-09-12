using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;

namespace OneDbgClient.Framework
{
    public class CommonViewModel : ViewModelBase
    {
        protected IMessageService MessageService { get; set; }
        protected IThemeService ThemeService { get; set; }

        public CommonViewModel()
        {
            MessageService = ServiceLocator.Current.GetInstance<IMessageService>();
            ThemeService = ServiceLocator.Current.GetInstance<IThemeService>();
        }
    }
}
