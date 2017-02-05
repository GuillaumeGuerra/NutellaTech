using VisitorModel.Business;
using VisitorModel.Visitors;

namespace VisitorModel.Elements
{
    public interface IFileSystemElement
    {
        void Visit(IVisitor visitor, VisitContext visitContext);
        int GetElementSize();
    }
}