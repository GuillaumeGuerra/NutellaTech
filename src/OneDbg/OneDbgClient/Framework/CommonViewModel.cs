using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using OneDbgClient.ViewModels;

namespace OneDbgClient.Framework
{
    public class CommonViewModel : ViewModelBase
    {
        public IPopupService PopupService { get; set; }

        public CommonViewModel()
        {
            PopupService = ServiceLocator.Current.GetInstance<IPopupService>();
        }
    }
}
