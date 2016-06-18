using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNet.Mvc;
using PatchManager.Model.Services;
using PatchManager.Models;
using PatchManager.Services.Model;
using PatchManager.Services.PatchActions;

namespace PatchManager.Controllers
{
    [Route("api/[controller]")]
    public class ReleasesController : Controller
    {
        public IModelService Model { get; set; }
        public IStatusResolverService StatusResolver { get; set; }
        public IPersistenceService Persistence { get; set; }

        public ReleasesController(IModelService model, IStatusResolverService statusResolver, IPersistenceService persistence)
        {
            Model = model;
            StatusResolver = statusResolver;
            Persistence = persistence;
        }

        [HttpGet]
        public IEnumerable<Release> GetAllReleases()
        {
            return Model.GetAllReleases();
        }

        [HttpGet]
        [Route("{releaseVersion}")]
        public Release GetRelease([FromRoute] string releaseVersion)
        {
            return Model.GetRelease(releaseVersion);
        }

        [HttpGet]
        [Route("{releaseVersion}/patches")]
        public IEnumerable<Patch> GetReleasePatches([FromRoute] string releaseVersion)
        {
            var release = Model.GetRelease(releaseVersion);
            var patches = Model.GetReleasePatches(releaseVersion);

            Parallel.ForEach(patches, patch =>
            {
                if (StatusResolver.ResolveIfOutdated(patch))
                    // NB : do not access the patch variable inside the task, or you will run into nasty race condition with the foreach enumeration
                    Task.Factory.StartNew(tmp => Persistence.UpdateReleasePatch(release, (Patch)tmp), patch.Patch);
            });

            return patches.Select(patch => patch.Patch);
        }

        [HttpGet]
        [Route("{releaseVersion}/patches/{patchId}")]
        public Patch GetReleasePatch([FromRoute] string releaseVersion, [FromRoute] int patchId)
        {
            var release = Model.GetRelease(releaseVersion);
            var patch = Model.GetReleasePatch(releaseVersion, patchId);

            StatusResolver.Resolve(patch);
            Task.Factory.StartNew(() => Persistence.UpdateReleasePatch(release, patch.Patch));

            return patch.Patch;
        }

        [HttpPost]
        [Route("{releaseVersion}/patches/")]
        public Patch PostPatchForRelease([FromRoute] string releaseVersion, [FromBody] Patch patch)
        {
            patch.Status.Registration = RegistrationStatus.Asked;
            patch.Status.Test = TestStatus.ToTest;

            Model.AddPatchToRelease(releaseVersion, patch);

            return patch;
        }

        [HttpPost]
        [Route("{releaseVersion}/patches/{patchId}/action/{actionToPerform}")]
        public Patch ApplyActionToPatch([FromRoute] string releaseVersion, [FromRoute] int patchId, [FromRoute] string actionToPerform)
        {
            var current = Model.GetReleasePatch(releaseVersion, patchId);
            if (current == null)
                return null;

            var action = Startup.Container.ResolveOptionalNamed<IPatchAction>(actionToPerform.ToUpper());

            if (action == null)
                throw new InvalidOperationException(string.Format("Unknown action to apply [{0}]", actionToPerform));

            if (action.Apply(current.Patch))
                Model.UpdateReleasePatch(releaseVersion, current.Patch);

            return current.Patch;
        }

        [HttpGet]
        [Route("{releaseVersion}/patches/{patchId}/action/preview")]
        public Patch PreviewPatch([FromRoute] string releaseVersion, [FromRoute] int patchId)
        {
            var patch = new PatchWithMetadata(new Patch()
            {
                Gerrit = new Gerrit()
                {
                    Id = patchId
                }
            });

            // Do not update the persistence layer here, as the gerrit doesn't exist yet ...
            StatusResolver.Resolve(patch);

            return patch.Patch;
        }
    }
}