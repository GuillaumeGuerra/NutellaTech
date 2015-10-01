using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace Sudoku.ViewModels
{
    public class ViewModelLocator
    {
        public MainPageViewModel MainPage => ServiceLocator.Current.GetInstance<MainPageViewModel>();
    }
}
