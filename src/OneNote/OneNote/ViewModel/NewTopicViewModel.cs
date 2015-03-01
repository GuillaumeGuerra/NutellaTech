using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using OneNote.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneNote.ViewModel
{
    public class NewTopicViewModel : ViewModelBase
    {
        public NewTopicViewModel()
        {
            Topic = new TopicViewModel();
        }

        private TopicViewModel _topic;
        public TopicViewModel Topic
        {
            get { return _topic; }
            set
            {
                _topic = value;
                RaisePropertyChanged("Topic");
            }
        }

        public ICommand CreateTopicCommand
        {
            get
            {
                return new RelayCommand(CreateTopic);
            }
        }

        private void CreateTopic()
        {
            using (var proxy = new NotificationServiceProxy())
            {
                Topic.Topic.Creator = App.Current.GetLocator().Settings.UserName;
                proxy.CreateTopic(Topic.Topic);
            }
        }
    }
}
