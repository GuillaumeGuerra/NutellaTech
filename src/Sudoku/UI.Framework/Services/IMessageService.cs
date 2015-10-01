using System;

namespace UI.Framework.Services
{
    public interface IMessageService
    {
        void ShowError(string title, string explanation, Exception exception);
        void ShowError(string title, Exception exception);
        void ShowInformation(string title, string text);
    }
}