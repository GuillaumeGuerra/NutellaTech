using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using PatchManager.Models;

namespace PatchManager.Controllers
{
    [Route("api/[controller]")]
    public class PatchesController : Controller
    {
        [HttpGet]
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

        [HttpGet]
        [Route("{patchVersion}")]
        public Patch GetPatch([FromRoute] string patchVersion)
        {
            return GetAllPatches().FirstOrDefault(patch => patch.Version == patchVersion);
        }

        [HttpGet]
        [Route("{patchVersion}/gerrits")]
        public IEnumerable<Gerrit> GetPatchGerrits([FromRoute] string patchVersion)
        {
            return new[]
            {
                new Gerrit()
                {
                    Id = 123,
                    Title = "Get rid of Jar-Jar once and for all",
                    Jira = "STR-123_" + patchVersion,
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
                    Jira = "STR-456_" + patchVersion,
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
                    Jira = "STR-789_" + patchVersion,
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

        [HttpGet]
        [Route("{patchVersion}/gerrits/{gerritId}")]
        public Gerrit GetPatchGerrit([FromRoute] string patchVersion, [FromRoute] int gerritId)
        {
            var result = GetPatchGerrits(patchVersion).FirstOrDefault(gerrit => gerrit.Id == gerritId);

            if (result != null)
                result.Status = new GerritStatus()
                {
                    Test = TestStatus.Tested,
                    Jira = JiraStatus.Resolved,
                    Merge = MergeStatus.Merged,
                    Patch = PatchStatus.Accepted
                };

            return result;
        }

        [HttpPost]
        [Route("{patchVersion}/gerrits/")]
        public Gerrit PostGerritForPatch([FromRoute] string patchVersion, [FromBody] Gerrit gerrit)
        {
            //TOTO : push it in the repository
            return gerrit;
        }

        [HttpGet]
        [Route("{patchVersion}/gerrits/{gerritId}/preview")]
        public Gerrit PreviewGerrit([FromRoute] string patchVersion, [FromRoute] int gerritId)
        {
            return new Gerrit()
            {
                Id = gerritId,
                Jira = "STW-123_"+gerritId,
                Owner = "ObiWan Kenobi",
                Title = "You have a nice beard",
                Status = new GerritStatus()
                {
                    Jira = JiraStatus.Open,
                    Merge = MergeStatus.ReadyForMerge
                }
            };
        }
    }
}