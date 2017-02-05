using System.Collections.Generic;
using VisitorModel.Business;
using VisitorModel.Visitors;

namespace VisitorModel.Elements
{
    public class DirectoryElement : FileSystemElement
    {
        public List<IFileSystemElement> Children { get; set; } = new List<IFileSystemElement>();

        public override int GetElementSize()
        {
            // Whatever its children, a directory doesn't weight anything in itself
            return 0;
        }

        public override void Visit(IVisitor visitor, VisitContext visitContext)
        {
            visitor.Inspect(this);

            Children.ForEach(element => element.Visit(visitor, visitContext));
        }

        public void AddChildren(params IFileSystemElement[] children)
        {
            Children.AddRange(children);
        }
    }
}