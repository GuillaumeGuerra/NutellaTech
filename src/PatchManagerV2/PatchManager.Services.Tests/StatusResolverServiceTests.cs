using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PatchManager.Model.Services;
using PatchManager.Models;
using PatchManager.Services.Model;
using PatchManager.Services.StatusResolver;

namespace PatchManager.Services.Tests
{
    [TestFixture]
    public class StatusResolverServiceTests
    {
        [Test]
        public void Should()
        {
            var jira = new Mock<IJiraService>(MockBehavior.Strict);
            var gerrit = new Mock<IGerritService>(MockBehavior.Strict);

            var resolver = new StatusResolverService(gerrit.Object, jira.Object);
            resolver.Resolve(new PatchWithMetadata(new Patch()
            {
                Gerrit = new Models.Gerrit() { Id = 123}
            }));

            jira.Verify();
            gerrit.Verify();
        }
    }
}
