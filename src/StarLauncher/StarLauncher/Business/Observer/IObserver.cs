using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLauncher.Business
{
    public enum MessageLevel
    {
        Information,
        Error
    }

    public interface IObserver
    {
        void PushMessage(string message, MessageLevel level);
    }
}
