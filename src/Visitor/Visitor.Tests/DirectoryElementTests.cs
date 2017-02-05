using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisitorModel.Business;
using VisitorModel.Elements;

namespace VisitorModel.Tests
{
    [TestClass]
    public class DirectoryElementTests
    {
        [TestMethod]
        public void TestThatWeOnlyVisitTheDirectoryWhenItsEmpty()
        {
            var root = new DirectoryElement();

            var visitor = Visit(root);

            Assert.AreEqual(1, visitor.VisitedElements.Count);
            Assert.AreSame(root, visitor.VisitedElements.First());
        }

        [TestMethod]
        public void TestThatWeVisitTheDirectoryAndAllTheChildrenWhenItContainsChildren()
        {
            var root = new DirectoryElement();
            var file1 = new FileElement();
            var file2 = new FileElement();
            root.AddChildren(file1, file2);

            var visitor = Visit(root);

            Assert.AreEqual(3, visitor.VisitedElements.Count);
            Assert.AreSame(root, visitor.VisitedElements[0]);
            Assert.AreSame(file1, visitor.VisitedElements[1]);
            Assert.AreSame(file2, visitor.VisitedElements[2]);
        }

        [TestMethod]
        public void TestThatTheSizeOfADirectoryIs0WhenItIsEmpty()
        {
            var root = new DirectoryElement();
            Assert.AreEqual(0, root.GetElementSize());
        }

        [TestMethod]
        public void TestThatTheSizeOfADirectoryIs0WhenItIsNotEmpty()
        {
            var root = new DirectoryElement();
            var file = new FileElement() { Content = new byte[100] };
            root.AddChildren(file);

            // first, we make sure the file has a non 0 size, to ensure the directory didn't ask its size to sum to itself
            Assert.IsTrue(file.GetElementSize() > 0);

            Assert.AreEqual(0, root.GetElementSize());
        }

        private static RecorderVisitor Visit(DirectoryElement root)
        {
            var visitor = new RecorderVisitor();
            root.Visit(visitor);

            return visitor;
        }
    }
}
