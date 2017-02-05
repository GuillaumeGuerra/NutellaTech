using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisitorModel.Business;
using VisitorModel.Elements;

namespace VisitorModel.Tests
{
    [TestClass]
    public class FileSystemHelpersTests
    {
        [TestMethod]
        public void TestThatTheSizeOfAnElementIsTheSumOfTheSizeOfAllChildren()
        {
            var root = new DirectoryElement();
            var file = new FileElement() { Content = new byte[100] };
            root.AddChildren(file);

            Assert.AreEqual(root.GetElementSize() + file.GetElementSize(), root.GetTotalSize());
        }

        [TestMethod]
        public void TestThatShortcutsAreExcludedFromTheComputationOfTheSize()
        {
            var root = new DirectoryElement();
            var file = new FileElement() { Content = new byte[100] };
            var shortcut = new ShortcutElement() { Target = file };
            root.AddChildren(shortcut);

            // The shortcut should be excluded from the computation, as technically speaking the file is not below the directory, so it should not impact its size
            Assert.AreEqual(root.GetElementSize(), root.GetTotalSize());
        }

        [TestMethod]
        public void TestThatAVisitWithNoContextUsesTheDefaultContext()
        {
            var recorderElement = new RecorderElement();
            recorderElement.Visit(null);

            Assert.AreSame(VisitContext.Default, recorderElement.Context);
        }

        [TestMethod]
        public void TestThatAllElementsAreListedWhenVisitingATree()
        {
            var root = new DirectoryElement();
            var file = new FileElement();
            var file2 = new FileElement();
            var shortcut = new ShortcutElement() { Target = file2 };

            root.AddChildren(file, shortcut);

            var list = root.GetAllElements().ToList();
            Assert.AreEqual(3, list.Count);
            Assert.AreSame(root, list[0]);
            Assert.AreSame(file, list[1]);
            Assert.AreSame(file2, list[2]);
        }

        [TestMethod]
        public void TestThatElementsAreListedOnlyOnceWhenVisitingATreeThatContainsCircularReferences()
        {
            var root = new DirectoryElement();
            var file = new FileElement();
            var file2 = new FileElement();
            var circularShortcut = new ShortcutElement() { Target = root }; // To create the circular reference
            var shortcut = new ShortcutElement() { Target = file2 };

            root.AddChildren(circularShortcut, file, shortcut);

            var list = root.GetAllElements().ToList();

            Assert.AreEqual(3, list.Count);
            Assert.AreSame(root, list[0]);
            Assert.AreSame(file, list[1]);
            Assert.AreSame(file2, list[2]);
        }
    }
}