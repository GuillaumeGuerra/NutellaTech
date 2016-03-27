using Moq;
using NUnit.Framework;
using PatchManager.Model.Services;
using PatchManager.Models;
using PatchManager.Services.PatchActions;
using PatchManager.TestFramework.Mock;

namespace PatchManager.Services.Tests.Actions
{
    [TestFixture]
    public class ResolveActionTests
    {
        [TestCase(JiraStatus.Resolved)]
        [TestCase(JiraStatus.Closed)]
        public void ShouldNotResolveWhenJiraIsNotResolvable(JiraStatus status)
        {
            // This mock is meant to make sure the jira service is not called
            using (var mock = new StrictMock<IJiraService>())
            {
                Patch actualPatch = new Patch()
                {
                    Status = new PatchStatus()
                    {
                        Jira = status
                    }
                };
                Assert.That(new ResolveAction(null).Apply(actualPatch), Is.False);

                // If the action chose not to apply, the status should remain the same
                Assert.That(actualPatch.Status.Jira, Is.EqualTo(status));
            }
        }

        [TestCase]
        public void ShouldResolveWhenJiraStatusIsResolvableAndResolveWorks()
        {
            using (var mock = new StrictMock<IJiraService>())
            {
                mock
                    .Setup(service => service.Resolve("TheJiraId"))
                    .Returns(true)
                    .Verifiable();

                var action = new ResolveAction(mock.Object);
                Patch actualPatch = new Patch()
                {
                    Status = new PatchStatus()
                    {
                        Jira = JiraStatus.Approved // To make sure the jira is actually resolvable
                    },
                    Jira = new Models.Jira()
                    {
                        Id = "TheJiraId"
                    }
                };

                Assert.That(action.Apply(actualPatch), Is.True);
                Assert.That(actualPatch.Status.Jira, Is.EqualTo(JiraStatus.Resolved));
            }
        }

        [TestCase]
        public void ShouldLeaveJiraStatusAsIsWhenJiraIsResolvableButResolveFails()
        {
            using (var mock = new StrictMock<IJiraService>())
            {
                mock
                    .Setup(service => service.Resolve("TheJiraId"))
                    .Returns(false) // returning false means that the resolve failed, for some reason
                    .Verifiable();

                var action = new ResolveAction(mock.Object);
                Patch actualPatch = new Patch()
                {
                    Status = new PatchStatus()
                    {
                        Jira = JiraStatus.Approved // To make sure the jira is actually resolvable
                    },
                    Jira = new Models.Jira()
                    {
                        Id = "TheJiraId"
                    }
                };

                // Even though the resolve failed, we don't really know the status, so it's safer to notify that an action could be performed
                Assert.That(action.Apply(actualPatch), Is.True);

                // By security, we should move the Jira status back to Unknown
                Assert.That(actualPatch.Status.Jira, Is.EqualTo(JiraStatus.Unknown));
            }
        }

    }

    [TestFixture]
    public class MergeActionTests
    {
        [TestCase(GerritStatus.BuildFailed)]
        [TestCase(GerritStatus.Merged)]
        [TestCase(GerritStatus.MissingBuild)]
        [TestCase(GerritStatus.MissingReviews)]
        [TestCase(GerritStatus.Unknown)]
        public void ShouldNotMergeWhenGerritIsNotMergeable(GerritStatus status)
        {
            // This mock is meant to make sure the gerrit service is not called
            using (var mock = new StrictMock<IGerritService>())
            {
                Patch actualPatch = new Patch()
                {
                    Status = new PatchStatus()
                    {
                        Gerrit = status
                    }
                };
                Assert.That(new MergeAction(mock.Object).Apply(actualPatch), Is.False);

                // If the action chose not to apply, the status should remain the same
                Assert.That(actualPatch.Status.Gerrit, Is.EqualTo(status));
            }
        }

        [TestCase]
        public void ShouldMergeWhenGerritStatusIsMergeableAndMergeWorks()
        {
            using (var mock = new StrictMock<IGerritService>())
            {
                mock
                    .Setup(service => service.Merge(42))
                    .Returns(true)
                    .Verifiable();

                var action = new MergeAction(mock.Object);
                Patch actualPatch = new Patch()
                {
                    Status = new PatchStatus()
                    {
                        Gerrit = GerritStatus.ReadyForMerge // To make sure the gerrit is actually resolvable
                    },
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 42
                    }
                };

                Assert.That(action.Apply(actualPatch), Is.True);
                Assert.That(actualPatch.Status.Gerrit, Is.EqualTo(GerritStatus.Merged));
            }
        }

        [TestCase]
        public void ShouldLeaveGerritStatusAsIsWhenGerritIsMeargeableButMergeFails()
        {
            using (var mock = new StrictMock<IGerritService>())
            {
                mock
                    .Setup(service => service.Merge(42))
                    .Returns(false) // returning false means that the merge failed, for some reason
                    .Verifiable();

                var action = new MergeAction(mock.Object);
                Patch actualPatch = new Patch()
                {
                    Status = new PatchStatus()
                    {
                        Gerrit = GerritStatus.ReadyForMerge // To make sure the gerrit is actually resolvable
                    },
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 42
                    }
                };

                // Even though the merge failed, we don't really know the status, so it's safer to notify that an action could be performed
                Assert.That(action.Apply(actualPatch), Is.True);

                // By security, we should move the gerrit status back to Unknown
                Assert.That(actualPatch.Status.Gerrit, Is.EqualTo(GerritStatus.Unknown));
            }
        }
    }
}