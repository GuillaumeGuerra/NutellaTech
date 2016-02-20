using System;
using System.Collections.Generic;
using PatchManager.Models;

namespace PatchManager.Services.PersistenceService
{
    /// <summary>
    /// This implementation exposes dummy values, to populate site with enough information to perform a demo
    /// </summary>
    internal class DemoPersistenceService : IPersistenceService
    {
        public IEnumerable<Patch> GetAllPatches()
        {
            return new[]
            {
                new Patch()
                {
                    Version = "19.8",
                    ReleaseManager = "Peter Goron",
                    ReleaseManagerMail = "peter.goron@sgcib.com",
                    Date = new DateTime(2015, 12, 16),
                    IsCurrent = false
                },
                new Patch()
                {
                    Version = "20.1",
                    ReleaseManager = "Guillaume Guerra",
                    ReleaseManagerMail = "guillaume.guerra@sgcib.com",
                    Date = new DateTime(2016, 12, 14),
                    IsCurrent = false
                },
                new Patch()
                {
                    Version = "20.2",
                    ReleaseManager = "Sullivan Veres",
                    ReleaseManagerMail = "sullivan.veres@sgcib.com",
                    Date = new DateTime(2018, 12, 16),
                    IsCurrent = true
                }
            };
        }

        public IEnumerable<Gerrit> GetGerrits(string patchVersion)
        {
            return new[]
            {
                new Gerrit()
                {
                    Id = 123,
                    Title = "Get rid of Jar-Jar once and for all",
                    Jira = new Jira()
                    {
                        Id = "STR-123_" + patchVersion,
                        Description = "Jira to get rid of Jar-Jar once and for all",
                    },
                    Status = new GerritStatus()
                    {
                        Patch = PatchStatus.Accepted,
                        Jira = JiraStatus.InProgress,
                        Merge = MergeStatus.MissingReviews,
                        Test = TestStatus.ToTest
                    }
                },
                new Gerrit()
                {
                    Id = 456,
                    Title = "Now that Jar-Jar is gone, time to take care of Anakin",
                    Jira = new Jira()
                    {
                        Id = "STR-456_" + patchVersion,
                        Description = "Jira to Get rid of Anakin, now that Jar-Jar is gone",
                    },
                    Status = new GerritStatus()
                    {
                        Patch = PatchStatus.Refused,
                        Jira = JiraStatus.Open,
                        Merge = MergeStatus.ReadyForMerge,
                        Test = TestStatus.Tested
                    }
                },
                new Gerrit()
                {
                    Id = 789,
                    Title =
                        "Revert of the deletion of Han Solo, finally the character is nice, I don't want him dead",
                    Jira = new Jira()
                    {
                        Id = "STR-789_" + patchVersion,
                        Description = "Jira to revert of the deletion of Han Solo, finally the character is nice, I don't want him dead",
                    },
                    Status = new GerritStatus()
                    {
                        Patch = PatchStatus.Asked,
                        Jira = JiraStatus.Resolved,
                        Merge = MergeStatus.Merged,
                        Test = TestStatus.Issue
                    }
                },
            };
        }

        public void AddGerritToPatch(Patch patch, Gerrit gerrit)
        {
        }
    }
}