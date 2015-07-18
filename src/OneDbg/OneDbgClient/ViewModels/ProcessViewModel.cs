using GalaSoft.MvvmLight;

namespace OneDbgClient.ViewModels
{
    public class ProcessViewModel : ViewModelBase
    {
        private int _pid;
        private string _name;

        public int PID
        {
            get { return _pid; }
            set
            {
                _pid = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
    }
}