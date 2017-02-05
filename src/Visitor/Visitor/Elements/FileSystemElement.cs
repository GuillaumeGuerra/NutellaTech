using VisitorModel.Business;
using VisitorModel.Visitors;

namespace VisitorModel.Elements
{
    public abstract class FileSystemElement : IFileSystemElement
    {
        public string Name { get; set; }

        public abstract int GetElementSize();

        public abstract void Visit(IVisitor visitor, VisitContext visitContext);
    }
}