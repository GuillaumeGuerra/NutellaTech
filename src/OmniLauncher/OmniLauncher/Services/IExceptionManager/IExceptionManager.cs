using System;

namespace OmniLauncher.Services.IExceptionManager
{
    internal interface IExceptionManager
    {
        void Show(Exception exception);
    }
}