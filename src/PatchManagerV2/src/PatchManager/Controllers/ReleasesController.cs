using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.AspNet.Mvc;
using PatchManager.Models;
using PatchManager.Services.GerritActions;
using PatchManager.Services.ModelService;
using PatchManager.Services.StatusResolverService;

namespace PatchManager.Controllers
{
    [Route("api/[controller]")]
    public class ReleasesController : Controller
    {
        public IModelService Model { get; set; }
        public IStatusResolverService StatusResolver { get; set; }

        public ReleasesController(IModelService model, IStatusResolverService statusResolver)
        {
            Model = model;
            StatusResolver = statusResolver;
        }

        [HttpGet]
        public IEnumerable<Patch> GetAllReleases()
        {
            return Model.GetAllPatches();
        }

        [HttpGet]
        [Route("{releaseVersion}")]
        public Patch GetRelease([FromRoute] string releaseVersion)
        {
            return Model.GetPatch(releaseVersion);
        }

        [HttpGet]
        [Route("{releaseVersion}/gerrits")]
        public IEnumerable<Gerrit> GetReleaseGerrits([FromRoute] string releaseVersion)
        {
            foreach (var gerrit in Model.GetPatchGerrits(releaseVersion))
            {
                StatusResolver.ResolveIfOutdated(gerrit);
                yield return gerrit.Gerrit;
            }
        }

        [HttpGet]
        [Route("{releaseVersion}/gerrits/{gerritId}")]
        public Gerrit GetReleaseGerrit([FromRoute] string releaseVersion, [FromRoute] int gerritId)
        {
            var gerrit = Model.GetGerritForPatch(releaseVersion, gerritId);

            StatusResolver.Resolve(gerrit);

            return gerrit.Gerrit;
        }

        [HttpPost]
        [Route("{releaseVersion}/gerrits/")]
        public Gerrit PostGerritForRelease([FromRoute] string releaseVersion, [FromBody] Gerrit gerrit)
        {
            gerrit.Status.Patch = PatchStatus.Asked;
            gerrit.Status.Test = TestStatus.ToTest;

            Model.AddGerritToPatch(releaseVersion, gerrit);

            return gerrit;
        }

        [HttpPost]
        [Route("{releaseVersion}/gerrits/{gerritId}/action/{actionToPerform}")]
        public Gerrit ApplyActionToGerrit([FromRoute] string releaseVersion, [FromRoute] int gerritId, [FromRoute] string actionToPerform)
        {
            var current = Model.GetGerritForPatch(releaseVersion, gerritId);
            if (current == null)
                return null;

            var action = Startup.Container.ResolveOptionalNamed<IGerritAction>(actionToPerform.ToUpper());

            if (action == null)
                throw new InvalidOperationException(string.Format("Unknown action to apply [{0}]", actionToPerform));

            if (action.Apply(current.Gerrit))
                Model.UpdatePatchGerrit(releaseVersion, current.Gerrit);

            return current.Gerrit;
        }

        [HttpGet]
        [Route("{releaseVersion}/gerrits/{gerritId}/action/preview")]
        public Gerrit PreviewGerrit([FromRoute] string releaseVersion, [FromRoute] int gerritId)
        {
            var gerrit = new GerritWithMetadata(new Gerrit()
            {
                Id = gerritId
            });
            StatusResolver.Resolve(gerrit);

            return gerrit.Gerrit;
        }
    }
}