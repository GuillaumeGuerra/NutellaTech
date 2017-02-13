using System;

namespace OmniLauncher.Services.IExceptionManager
{
    public interface IMessageService
    {
        void ShowException(Exception exception);
    }
}