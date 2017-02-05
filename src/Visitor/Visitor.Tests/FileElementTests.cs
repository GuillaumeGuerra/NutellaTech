using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisitorModel.Business;
using VisitorModel.Elements;

namespace VisitorModel.Tests
{
    [TestClass]
    public class FileElementTests
    {
        [TestMethod]
        public void TestThatTheFileIsVisited()
        {
            var file = new FileElement();

            var visitor = new RecorderVisitor();
            file.Visit(visitor);

            Assert.AreEqual(1, visitor.VisitedElements.Count);
            Assert.AreSame(file, visitor.VisitedElements.First());
        }

        [TestMethod]
        public void TestThatTheSizeOfAFileIsTheLengthOfItsContent()
        {
            var file = new FileElement() { Content = new byte[100] };
            Assert.AreEqual(100, file.GetElementSize());
        }
    }
}
