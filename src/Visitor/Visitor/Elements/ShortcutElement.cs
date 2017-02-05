using VisitorModel.Business;
using VisitorModel.Visitors;

namespace VisitorModel.Elements
{
    public class ShortcutElement : FileSystemElement
    {
        public override int GetElementSize()
        {
            // Whatever its target, a shortuct doesn't weight anything in itself
            return 0;
        }

        public override void Visit(IVisitor visitor, VisitContext visitContext)
        {
            if (!visitContext.SkipShortcuts)
                Target.Visit(visitor, visitContext);
        }

        public IFileSystemElement Target { get; set; }
    }
}