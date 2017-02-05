using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisitorModel.Elements;
using VisitorModel.Visitors;

namespace VisitorModel.Tests
{
    [TestClass]
    public class UniqueVisitorTests
    {
        [TestMethod]
        public void TestThatVisitIsMadeOnlyOncePerElement()
        {
            var recorder = new RecorderVisitor();
            var visitor = new UniqueVisitor(recorder);

            var file = new FileElement();
            var directory = new DirectoryElement();
            visitor.Inspect(file);
            visitor.Inspect(directory);
            visitor.Inspect(file); // Now we try to make the viitor inspect another time the same element, it shouldn't let the inner visitor inspect it

            Assert.AreEqual(2, recorder.VisitedElements.Count);
            Assert.AreSame(file, recorder.VisitedElements.First());
            Assert.AreSame(directory, recorder.VisitedElements.Last());
        }
    }
}