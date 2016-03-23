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
}