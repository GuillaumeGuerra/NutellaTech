using System.Collections.Generic;
using VisitorModel.Elements;

namespace VisitorModel.Visitors
{
    public class UniqueVisitor : IVisitor
    {
        private IVisitor InnerVisitor { get; }
        private HashSet<IFileSystemElement> AllItems { get; } = new HashSet<IFileSystemElement>(new ReferenceComparer());

        public UniqueVisitor(IVisitor innerVisitor)
        {
            InnerVisitor = innerVisitor;
        }

        public void Inspect(IFileSystemElement element)
        {
            if (AllItems.Contains(element))
                return;

            AllItems.Add(element);
            InnerVisitor.Inspect(element);
        }

        private class ReferenceComparer : IEqualityComparer<IFileSystemElement>
        {
            public bool Equals(IFileSystemElement x, IFileSystemElement y)
            {
                return object.ReferenceEquals(x, y);
            }

            public int GetHashCode(IFileSystemElement obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}