using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using PatchManager.Models;
using PatchManager.Services.GerritService;
using PatchManager.Services.JiraService;
using PatchManager.Services.ModelService;

namespace PatchManager.Controllers
{
    [Route("api/[controller]")]
    public class PatchesController : Controller
    {
        public IModelService Model { get; set; }
        public IGerritService Gerrit { get; set; }
        public IJiraService Jira { get; set; }

        public PatchesController(IModelService model, IGerritService gerrit, IJiraService jira)
        {
            Model = model;
            Gerrit = gerrit;
            Jira = jira;
        }

        [HttpGet]
        public IEnumerable<Patch> GetAllPatches()
        {
            return Model.GetAllPatches();
        }

        [HttpGet]
        [Route("{patchVersion}")]
        public Patch GetPatch([FromRoute] string patchVersion)
        {
            return Model.GetPatch(patchVersion);
        }

        [HttpGet]
        [Route("{patchVersion}/gerrits")]
        public IEnumerable<Gerrit> GetPatchGerrits([FromRoute] string patchVersion)
        {
            return Model.GetPatchGerrits(patchVersion);
        }

        [HttpGet]
        [Route("{patchVersion}/gerrits/{gerritId}")]
        public Gerrit GetPatchGerrit([FromRoute] string patchVersion, [FromRoute] int gerritId)
        {
            var gerrit = Model.GetGerritForPatch(patchVersion, gerritId);

            var gerritMetadata = Gerrit.GetGerritMetadata(gerritId);
            var jiraMetadata = Jira.GetJiraMetadata(gerritMetadata.JiraId);

            gerrit.Status.Merge = gerritMetadata.Status;
            gerrit.Status.Jira = jiraMetadata.Status;

            return gerrit;
        }

        [HttpPost]
        [Route("{patchVersion}/gerrits/")]
        public Gerrit PostGerritForPatch([FromRoute] string patchVersion, [FromBody] Gerrit gerrit)
        {
            gerrit.Status.Patch = PatchStatus.Asked;
            gerrit.Status.Test = TestStatus.ToTest;

            Model.AddGerritToPatch(patchVersion, gerrit);

            return gerrit;
        }

        [HttpGet]
        [Route("{patchVersion}/gerrits/{gerritId}/preview")]
        public Gerrit PreviewGerrit([FromRoute] string patchVersion, [FromRoute] int gerritId)
        {
            var gerritMetadata = Gerrit.GetGerritMetadata(gerritId);
            var jiraMetadata = Jira.GetJiraMetadata(gerritMetadata.JiraId);

            return new Gerrit()
            {
                Id = gerritId,
                Jira = gerritMetadata.JiraId,
                Owner = gerritMetadata.Owner,
                Title = gerritMetadata.Title,
                Status = new GerritStatus()
                {
                    Jira = jiraMetadata.Status,
                    Merge = gerritMetadata.Status
                }
            };
        }
    }
}