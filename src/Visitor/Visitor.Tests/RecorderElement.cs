using VisitorModel.Business;
using VisitorModel.Elements;
using VisitorModel.Visitors;

namespace VisitorModel.Tests
{
    public class RecorderElement : IFileSystemElement
    {
        public void Visit(IVisitor visitor, VisitContext visitContext)
        {
            Context = visitContext;
        }

        public VisitContext Context { get; set; }

        public int GetElementSize()
        {
            return 0;
        }
    }
}