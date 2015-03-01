using GalaSoft.MvvmLight.Command;
using Model;
using OneNote.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneNote.ViewModel
{
    public class VoteNotificationViewModel : NotificationViewModel
    {
        private VoteNotification VoteNotification
        {
            get { return Notification as VoteNotification; }
        }

        public string VotesForCount
        {
            get { return string.Format("+{0}", VoteNotification.Voters.Count(v => v.VoteFor)); }
        }

        public string VotesAgainstCount
        {
            get { return string.Format("-{0}", VoteNotification.Voters.Count(v => !v.VoteFor)); }
        }

        public ICommand VoteForCommand
        {
            get { return new RelayCommand(VoteFor); }
        }

        public ICommand VoteAgainstCommand
        {
            get { return new RelayCommand(VoteAgainst); }
        }

        private void VoteFor()
        {
            using (var proxy = new NotificationServiceProxy())
            {
                proxy.VoteOnNotification(VoteNotification, true, App.Current.GetLocator().Settings.UserName);
            }
        }

        private void VoteAgainst()
        {
            using (var proxy = new NotificationServiceProxy())
            {
                proxy.VoteOnNotification(VoteNotification, false, App.Current.GetLocator().Settings.UserName);
            }
        }
    }
}
