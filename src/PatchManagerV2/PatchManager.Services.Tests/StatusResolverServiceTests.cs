using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PatchManager.Model.Services;
using PatchManager.Models;
using PatchManager.Services.Gerrit;
using PatchManager.Services.Jira;
using PatchManager.Services.Model;
using PatchManager.Services.StatusResolver;
using PatchManager.Services.Tests.Framework;

namespace PatchManager.Services.Tests
{
    [TestFixture]
    public class StatusResolverServiceTests
    {
        private PatchManagerContextMock _context;

        [SetUp]
        public void SetUp()
        {
            _context = new PatchManagerContextMock().WithFrozenSystemDate();
        }

        [Test]
        public void ShouldFetchJiraAndGerritInformationWhenResolvingPatch()
        {
            var jira = new Mock<IJiraService>(MockBehavior.Strict);
            var gerrit = new Mock<IGerritService>(MockBehavior.Strict);

            jira
                .Setup(mock => mock.GetJiraInformation("TheJiraId"))
                .Returns(new JiraInformation()
                {
                    Description = "Yoda is very small",
                    Id = "TheJiraId",
                    Status = JiraStatus.Approved
                })
                .Verifiable();

            gerrit
                .Setup(mock => mock.GetGerritInformation(123))
                .Returns(new GerritInformation()
                {
                    JiraId = "TheJiraId",
                    Owner = "Yoda",
                    Title = "Yoda has very large ears",
                    Status = MergeStatus.MissingBuild
                });

            var actualPatch = new PatchWithMetadata(new Patch()
            {
                Gerrit = new Models.Gerrit() { Id = 123 }
            });

            // Technically we don't care about this date, since we force a resolution of gerrit and jira
            // Yet, the idea is to make sure this date is properly assigned
            actualPatch.LastRefresh = DateTime.MinValue;

            new StatusResolverService(_context, gerrit.Object, jira.Object).Resolve(actualPatch);

            // Since the resolution happened, the last resolution should have been logged in the object
            Assert.That(actualPatch.LastRefresh, Is.EqualTo(_context.Now));

            jira.Verify();
            gerrit.Verify();
        }

        [Test]
        public void ShouldNotFetchJiraNorGerritWhenJiraIsClosedAndGerritIsMerged()
        {
            var jira = new Mock<IJiraService>(MockBehavior.Strict);
            var gerrit = new Mock<IGerritService>(MockBehavior.Strict);

            // No expectation, since the services shouln't be called

            new StatusResolverService(_context, gerrit.Object, jira.Object).Resolve(new PatchWithMetadata(new Patch()
            {
                Gerrit = new Models.Gerrit() { Id = 123 },
                Status = new PatchStatus()
                {
                    Jira = JiraStatus.Closed,
                    Merge = MergeStatus.Merged
                }
            }));

            jira.Verify();
            gerrit.Verify();
        }

        [TestCase(-10,false)]
        [TestCase(-1, true)]
        public void ShouldOnlyResolveGerritNorJiraWhenLastRefreshWasLongEnough(int lastRefresh,bool expectedResolve)
        {
            var jira = new Mock<IJiraService>(MockBehavior.Strict);
            var gerrit = new Mock<IGerritService>(MockBehavior.Strict);

            if (expectedResolve)
            {
                jira
                    .Setup(mock => mock.GetJiraInformation("TheJiraId"))
                    .Returns(new JiraInformation()
                    {
                        Description = "Yoda is very small",
                        Id = "TheJiraId",
                        Status = JiraStatus.Approved
                    })
                    .Verifiable();

                gerrit
                    .Setup(mock => mock.GetGerritInformation(123))
                    .Returns(new GerritInformation()
                    {
                        JiraId = "TheJiraId",
                        Owner = "Yoda",
                        Title = "Yoda has very large ears",
                        Status = MergeStatus.MissingBuild
                    });
            }

            var actualPatch = new PatchWithMetadata(new Patch()
            {
                Gerrit = new Models.Gerrit() { Id = 123 }
            });

            var initialLastRefresh = _context.Now.AddMinutes(-lastRefresh);
            actualPatch.LastRefresh = initialLastRefresh;

            new StatusResolverService(_context, gerrit.Object, jira.Object).ResolveIfOutdated(actualPatch);

            if (expectedResolve)
                Assert.That(actualPatch.LastRefresh, Is.EqualTo(_context.Now));
            else
                Assert.That(actualPatch.LastRefresh, Is.EqualTo(initialLastRefresh));

            jira.Verify();
            gerrit.Verify();
        }
    }
}
