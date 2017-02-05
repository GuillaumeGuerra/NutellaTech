using System.Collections.Generic;
using VisitorModel.Elements;
using VisitorModel.Visitors;

namespace VisitorModel.Tests
{
    public class RecorderVisitor : IVisitor
    {
        public void Inspect(IFileSystemElement element)
        {
            VisitedElements.Add(element);
        }

        public List<IFileSystemElement> VisitedElements { get; set; } = new List<IFileSystemElement>();
    }
}