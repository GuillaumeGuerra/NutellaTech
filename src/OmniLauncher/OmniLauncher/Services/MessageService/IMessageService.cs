using System;

namespace OmniLauncher.Services.IExceptionManager
{
    internal interface IMessageService
    {
        void ShowException(Exception exception);
    }
}