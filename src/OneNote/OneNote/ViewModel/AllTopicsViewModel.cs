using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Model;
using OneNote.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneNote.ViewModel
{
    public class AllTopicsViewModel : ViewModelBase
    {
        public AllTopicsViewModel()
        {
            if (IsInDesignMode)
            {
                AllTopics =
                    new ObservableCollection<TopicViewModel>()
                    {
                        new TopicViewModel(){Topic=new Topic(){Creator="Creator 1",Name="Topic 1"}},
                        new TopicViewModel(){Topic=new Topic(){Creator="Creator 2",Name="Topic 2"}},
                        new TopicViewModel(){Topic=new Topic(){Creator="Creator 3",Name="Topic 3"}}
                    };
            }
            else
            {
                using (var proxy = new NotificationServiceProxy())
                {
                    AllTopics = new ObservableCollection<TopicViewModel>(proxy.GetAllTopics().Select(t => new TopicViewModel() { Topic = t }));
                }
            }
        }

        private ObservableCollection<TopicViewModel> _allTopics;
        public ObservableCollection<TopicViewModel> AllTopics
        {
            get { return _allTopics; }
            set
            {
                _allTopics = value;
                RaisePropertyChanged("AllTopics");
            }
        }
    }
}
