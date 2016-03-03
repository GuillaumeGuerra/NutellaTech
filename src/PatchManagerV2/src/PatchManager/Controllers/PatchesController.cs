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

        [HttpPost]
        [Route("{patchVersion}/gerrits/{gerritId}/action/{actionToPerform}")]
        public Gerrit ApplyActionToGerrit([FromRoute] string patchVersion, [FromRoute] int gerritId, [FromRoute] string actionToPerform)
        {
            var current = Model.GetGerritForPatch(patchVersion, gerritId);
            if (current == null)
                return null;

            var action = Startup.Container.ResolveOptionalNamed<IGerritAction>(actionToPerform.ToUpper());

            if (action == null)
                throw new InvalidOperationException(string.Format("Unknown action to apply [{0}]", actionToPerform));

            if (action.Apply(current.Gerrit))
                Model.UpdatePatchGerrit(patchVersion, current.Gerrit);

            return current.Gerrit;
        }
    }
}