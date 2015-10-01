using System;

namespace UI.Framework.Services
{
    public class MessageService : IMessageService
    {
        public void ShowError(string title, string explanation, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void ShowError(string title, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void ShowInformation(string title, string text)
        {
            throw new NotImplementedException();
        }
    }
}