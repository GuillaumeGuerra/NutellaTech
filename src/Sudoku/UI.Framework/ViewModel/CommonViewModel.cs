using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using UI.Framework.Services;

namespace UI.Framework.ViewModel
{
    public class CommonViewModel : ViewModelBase
    {
        protected IMessageService MessageService { get; set; }

        public CommonViewModel()
        {
            MessageService = ServiceLocator.Current.GetInstance<IMessageService>();
        }
    }
}
