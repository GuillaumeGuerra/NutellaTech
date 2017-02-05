using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorModel.Elements;
using VisitorModel.Visitors;

namespace VisitorModel.Business
{
    public static class FileSystemHelpers
    {
        public static int GetTotalSize(this IFileSystemElement element)
        {
            var size = 0;
            var visitor = new ActionVisitor(systemElement => size += systemElement.GetElementSize());
            var uniqueVisitor = new UniqueVisitor(visitor); // This will ensure that the visit of the tree ignores cycles, by ignoring items that were already visited

            element.Visit(uniqueVisitor, new VisitContext()
            {
                SkipShortcuts = true // When computing the size, we shoult ignore the shortcuts
            });

            return size;
        }

        public static void Visit(this IFileSystemElement element, IVisitor visitor)
        {
            element.Visit(visitor, VisitContext.Default);
        }

        public static IEnumerable<IFileSystemElement> GetAllElements(this IFileSystemElement element)
        {
            return new FileSystemElementEnumerable(element);
        }

        private class FileSystemElementEnumerable : IEnumerable<IFileSystemElement>
        {
            private IEnumerator<IFileSystemElement> Enumerator { get; set; }

            public FileSystemElementEnumerable(IFileSystemElement element)
            {
                Enumerator = new FileSystemElementEnumerator(element);
            }

            public IEnumerator<IFileSystemElement> GetEnumerator()
            {
                return Enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class FileSystemElementEnumerator : IEnumerator<IFileSystemElement>
        {
            private IFileSystemElement CurrentElement { get; set; }
            private IFileSystemElement Element { get; set; }
            private BlockingCollection<IFileSystemElement> Collection { get; set; }

            public FileSystemElementEnumerator(IFileSystemElement element)
            {
                Element = element;
                Collection = new BlockingCollection<IFileSystemElement>(1); // 1 to block the generation of items until we could consume them
                Task.Factory.StartNew(() =>
                {
                    var visitor = new ActionVisitor(systemElement => Collection.Add(systemElement));

                    element.Visit(visitor);
                    Collection.CompleteAdding();
                });
            }

            public void Dispose()
            {
                Collection.Dispose();
            }

            public bool MoveNext()
            {
                if (Collection.IsCompleted)
                {
                    CurrentElement = null;
                    return false;
                }

                IFileSystemElement tmp;
                if (!Collection.TryTake(out tmp, -1))
                {
                    CurrentElement = null;
                    return false;
                }

                CurrentElement = tmp;

                return true;
            }

            public void Reset()
            {
                // TODO : ???
            }

            public IFileSystemElement Current { get { return CurrentElement; } }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}