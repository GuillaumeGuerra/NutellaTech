using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneNote.ViewModel
{
    public class ChosenTopicsViewModel : ViewModelBase
    {
        public ChosenTopicsViewModel()
        {
            ChosenTopics = new ObservableCollection<TopicNotificationsViewModel>();
        }

        private ObservableCollection<TopicNotificationsViewModel> _chosenTopics;
        public ObservableCollection<TopicNotificationsViewModel> ChosenTopics
        {
            get { return _chosenTopics; }
            set
            {
                _chosenTopics = value;
                RaisePropertyChanged("ChosenTopics");
            }
        }

    }
}
