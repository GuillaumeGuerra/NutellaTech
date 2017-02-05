using VisitorModel.Business;
using VisitorModel.Visitors;

namespace VisitorModel.Elements
{
    public class FileElement : FileSystemElement
    {
        public override int GetElementSize()
        {
            return Content.Length;
        }

        public override void Visit(IVisitor visitor, VisitContext visitContext)
        {
            visitor.Inspect(this);
        }

        public byte[] Content { get; set; }
    }
}