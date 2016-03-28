using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatchManager.Framework;
using PatchManager.Model.Services;
using PatchManager.Models;
using PatchManager.Services.Context;

namespace PatchManager.Services.Persistence
{
    public class LocalFilesPersistenceService : IPersistenceService
    {
        public IPatchManagerContextService Context { get; set; }

        public LocalFilesPersistenceService(IPatchManagerContextService context)
        {
            Context = context;
            if (!Directory.Exists(context.Settings.PersistenceDirectoryPath))
                throw new InvalidOperationException(
                    $"Unknown directory for persistence [{context.Settings.PersistenceDirectoryPath}]");
        }

        public IEnumerable<Release> GetAllReleases()
        {
            return Directory
                .GetFiles(Context.Settings.PersistenceDirectoryPath, "*.json")
                .Select(release => TryDeserialize<Release>(release));
        }

        public IEnumerable<Patch> GetPatches(string releaseVersion)
        {
            var patchesPath = Path.Combine(Context.Settings.PersistenceDirectoryPath, releaseVersion);
            if (!Directory.Exists(patchesPath))
                return new Patch[0];

            return Directory
                .GetFiles(patchesPath)
                .Select(patch => TryDeserialize<Patch>(patch));
        }

        public void AddPatchToRelease(Release release, Patch patch)
        {
            throw new NotImplementedException();
        }

        public void UpdateReleasePatch(Release release, Patch patch)
        {
            throw new NotImplementedException();
        }

        private T TryDeserialize<T>(string filePath)
        {
            try
            {
                return JsonHelper<T>.JsonToObject(File.ReadAllText(filePath));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to read json content from file [{filePath}]", e);
            }
        }
    }
}
