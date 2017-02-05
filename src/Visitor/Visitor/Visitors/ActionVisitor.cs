using System;
using VisitorModel.Elements;

namespace VisitorModel.Visitors
{
    public class ActionVisitor : IVisitor
    {
        public Action<IFileSystemElement> Action { get; set; }

        public ActionVisitor(Action<IFileSystemElement> action)
        {
            Action = action;
        }

        public void Inspect(IFileSystemElement element)
        {
            Action(element);
        }
    }
}