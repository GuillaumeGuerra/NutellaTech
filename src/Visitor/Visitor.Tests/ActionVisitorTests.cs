using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisitorModel.Business;
using VisitorModel.Elements;
using VisitorModel.Visitors;

namespace VisitorModel.Tests
{
    [TestClass]
    public class ActionVisitorTests
    {
        [TestMethod]
        public void TestThatTheFuncIsCalledForEachVisitedElement()
        {
            var visitedElements = new List<IFileSystemElement>();
            var visitor = new ActionVisitor(element => visitedElements.Add(element));

            var root = new DirectoryElement();
            var file = new FileElement();
            root.AddChildren(file);

            root.Visit(visitor);

            Assert.AreEqual(2, visitedElements.Count);
            Assert.AreSame(root, visitedElements.First());
            Assert.AreSame(file, visitedElements.Last());
        }
    }
}
