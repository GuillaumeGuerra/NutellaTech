using System;
using System.Collections.Generic;
using PatchManager.Model.Services;
using PatchManager.Models;

namespace PatchManager.Services.Persistence
{
    /// <summary>
    /// This implementation exposes dummy values, to populate site with enough information to perform a demo
    /// </summary>
    internal class DemoPersistenceService : IPersistenceService
    {
        public IEnumerable<Release> GetAllReleases()
        {
            return new[]
            {
                new Release()
                {
                    Version = "19.8",
                    ReleaseManager = "Peter Goron",
                    ReleaseManagerMail = "peter.goron@sgcib.com",
                    Date = new DateTime(2015, 12, 16),
                    IsCurrent = false
                },
                new Release()
                {
                    Version = "20.1",
                    ReleaseManager = "Guillaume Guerra",
                    ReleaseManagerMail = "guillaume.guerra@sgcib.com",
                    Date = new DateTime(2016, 12, 14),
                    IsCurrent = false
                },
                new Release()
                {
                    Version = "20.2",
                    ReleaseManager = "Sullivan Veres",
                    ReleaseManagerMail = "sullivan.veres@sgcib.com",
                    Date = new DateTime(2018, 12, 16),
                    IsCurrent = true
                }
            };
        }

        public IEnumerable<Patch> GetPatches(string releaseVersion)
        {
            return new[]
            {
                new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 123,
                        Description = "Get rid of Jar-Jar once and for all"
                    },
                    Jira = new Models.Jira()
                    {
                        Id = "STR-123_" + releaseVersion,
                        Description = "Jira to get rid of Jar-Jar once and for all",
                    },
                    Asset = RiskOneAsset.Core,
                    Owner = "Kylo Ren",
                    Status = new PatchStatus()
                    {
                        Registration = RegistrationStatus.Accepted,
                        Jira = JiraStatus.InProgress,
                        Merge = MergeStatus.MissingReviews,
                        Test = TestStatus.ToTest
                    }
                },
                new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 456,
                        Description = "Now that Jar-Jar is gone, time to take care of Anakin"
                    },
                    Jira = new Models.Jira()
                    {
                        Id = "STR-456_" + releaseVersion,
                        Description = "Jira to Get rid of Anakin, now that Jar-Jar is gone",
                    },
                    Asset = RiskOneAsset.Official,
                    Owner = "Finn",
                    Status = new PatchStatus()
                    {
                        Registration = RegistrationStatus.Refused,
                        Jira = JiraStatus.Open,
                        Merge = MergeStatus.ReadyForMerge,
                        Test = TestStatus.Tested
                    }
                },
                new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 789,
                        Description = "Revert of the deletion of Han Solo, finally the character is nice, I don't want him dead"
                    },
                    Jira = new Models.Jira()
                    {
                        Id = "STR-789_" + releaseVersion,
                        Description = "Jira to revert of the deletion of Han Solo, finally the character is nice, I don't want him dead",
                    },
                    Asset = RiskOneAsset.Rates,
                    Owner = "Rey",
                    Status = new PatchStatus()
                    {
                        Registration = RegistrationStatus.Asked,
                        Jira = JiraStatus.Resolved,
                        Merge = MergeStatus.Merged,
                        Test = TestStatus.Issue
                    }
                },
                new Patch()
                {
                    Gerrit = new Models.Gerrit()
                    {
                        Id = 666,
                        Description = "Rey, we don't know yet if you're Leia's daughter, but you're as pretty as she was, a long time ago, in a galaxy far far away"
                    },
                    Jira = new Models.Jira()
                    {
                        Id = "STR-666_" + releaseVersion,
                        Description = "Let's find another fancy bikini, as it's the only way to decide who is the prettiest"
                    },
                    Asset = RiskOneAsset.Repo,
                    Owner = "Poe Dameron",
                    Status = new PatchStatus()
                    {
                        Registration = RegistrationStatus.Asked,
                        Jira = JiraStatus.Resolved,
                        Merge = MergeStatus.Merged,
                        Test = TestStatus.Issue
                    }
                },
            };
        }

        public void AddPatchToRelease(Release release, Patch patch)
        {
        }

        public void UpdateReleasePatch(Release release, Patch patch)
        {
        }
    }
}