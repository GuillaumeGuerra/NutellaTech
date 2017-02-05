using VisitorModel.Elements;

namespace VisitorModel.Visitors
{
    public interface IVisitor
    {
        void Inspect(IFileSystemElement element);
    }
}