using System;

namespace OneDbgClient.Framework
{
    public interface IMessageService
    {
        void ShowError(string title, string explanation, Exception exception);
        void ShowError(string title, Exception exception);
        void ShowInformation(string title, string text);
    }
}