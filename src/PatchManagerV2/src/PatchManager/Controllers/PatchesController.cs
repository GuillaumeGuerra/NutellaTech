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
            var result = GetAllPatches().FirstOrDefault(patch => patch.Version == patchVersion);

            if (result != null)
                result.Gerrits = new[]
                {
                    new Gerrit()
                    {
                        Id = 123,
                        Title = "Get rid of Jar-Jar once and for all",
                        Jira = "STR-123"
                    },
                    new Gerrit()
                    {
                        Id = 456,
                        Title = "Now that Jar-Jar is gone, time to take care of Anakin",
                        Jira = "STR-456"
                    },
                    new Gerrit()
                    {
                        Id = 789,
                        Title = "Revert of the deletion of Han Solo, finally the character is nice, I don't want him dead",
                        Jira = "STR-789"
                    },
                };

            return result;
        }
    }

    public class Gerrit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Jira { get; set; }
    }
}