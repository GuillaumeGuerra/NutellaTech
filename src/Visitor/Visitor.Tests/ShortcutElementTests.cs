using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisitorModel.Business;
using VisitorModel.Elements;

namespace VisitorModel.Tests
{
    [TestClass]
    public class ShortcutElementTests
    {
        [TestMethod]
        public void TestThatTheTargetIsVisitedWhenTheShortcutIsVisited()
        {
            var shortcut = new ShortcutElement();
            var file = new FileElement();
            shortcut.Target = file;

            var visitor = new RecorderVisitor();
            shortcut.Visit(visitor);

            Assert.AreEqual(1, visitor.VisitedElements.Count);
            Assert.AreSame(file, visitor.VisitedElements.First());
        }

        [TestMethod]
        public void TestThatTheSizeOfAShortcutIs0()
        {
            var shortcut = new ShortcutElement();
            var file = new FileElement() { Content = new byte[100] };
            shortcut.Target = file;

            // first, we make sure the file has a non 0 size, to ensure the shortcut didn't ask its size to sum to itself
            Assert.IsTrue(file.GetElementSize() > 0);

            Assert.AreEqual(0, shortcut.GetElementSize());
        }

        [TestMethod]
        public void TestThatTheTargetIsNotVisitedWhenTheVisitContextSpecifiesNotToVisitShortcuts()
        {
            var shortcut = new ShortcutElement();
            var file = new FileElement();
            shortcut.Target = file;

            var visitor = new RecorderVisitor();
            shortcut.Visit(visitor, new VisitContext() { SkipShortcuts = true });

            Assert.AreEqual(0, visitor.VisitedElements.Count);
        }
    }
}