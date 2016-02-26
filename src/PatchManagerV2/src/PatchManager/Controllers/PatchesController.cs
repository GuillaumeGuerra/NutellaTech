using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using PatchManager.Models;
using PatchManager.Services.GerritService;
using PatchManager.Services.JiraService;
using PatchManager.Services.ModelService;
using PatchManager.Services.StatusResolverService;

namespace PatchManager.Controllers
{
    [Route("api/[controller]")]
    public class PatchesController : Controller
    {
        public IModelService Model { get; set; }
        public IStatusResolverService StatusResolver { get; set; }

        public PatchesController(IModelService model, IStatusResolverService statusResolver)
        {
            Model = model;
            StatusResolver = statusResolver;
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
            foreach (var gerrit in Model.GetPatchGerrits(patchVersion))
            {
                StatusResolver.ResolveIfOutdated(gerrit);
                yield return gerrit.Gerrit;
            }
        }

        [HttpGet]
        [Route("{patchVersion}/gerrits/{gerritId}")]
        public Gerrit GetPatchGerrit([FromRoute] string patchVersion, [FromRoute] int gerritId)
        {
            var gerrit = Model.GetGerritForPatch(patchVersion, gerritId);

            StatusResolver.Resolve(gerrit);

            return gerrit.Gerrit;
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
        [Route("{patchVersion}/gerrits/{gerritId}/action/preview")]
        public Gerrit PreviewGerrit([FromRoute] string patchVersion, [FromRoute] int gerritId)
        {
            var gerrit = new GerritWithMetadata(new Gerrit()
            {
                Id = gerritId
            });
            StatusResolver.Resolve(gerrit);

            return gerrit.Gerrit;
        }

        [HttpPost]
        [Route("{patchVersion}/gerrits/{gerritId}/action/accept")]
        public Gerrit AcceptGerrit([FromRoute] string patchVersion, [FromRoute] int gerritId, [FromRoute] string actionToPerform)
        {
            return ApplyActionToGerrit(patchVersion, gerritId, gerrit =>
            {
                if (gerrit.Status.Patch == PatchStatus.Accepted)
                    return false;

                gerrit.Status.Patch = PatchStatus.Accepted;
                return true;
            });
        }

        [HttpPost]
        [Route("{patchVersion}/gerrits/{gerritId}/action/refuse")]
        public Gerrit RefuseGerrit([FromRoute] string patchVersion, [FromRoute] int gerritId, [FromRoute] string actionToPerform)
        {
            return ApplyActionToGerrit(patchVersion, gerritId, gerrit =>
            {
                if (gerrit.Status.Patch == PatchStatus.Refused)
                    return false;

                gerrit.Status.Patch = PatchStatus.Refused;
                return true;
            });
        }

        private Gerrit ApplyActionToGerrit(string patchVersion, int gerritId, Func<Gerrit,bool> func )
        {
            var current = Model.GetGerritForPatch(patchVersion, gerritId);
            if (current == null)
                return null;

            if(func(current.Gerrit))
                Model.UpdatePatchGerrit(patchVersion, current.Gerrit);

            return current.Gerrit;
        }
    }
}