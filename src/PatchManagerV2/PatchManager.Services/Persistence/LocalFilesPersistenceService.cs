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
                .Select(release => DeserializeOrThrow<Release>(release));
        }

        public IEnumerable<Patch> GetPatches(string releaseVersion)
        {
            var patchesPath = Path.Combine(Context.Settings.PersistenceDirectoryPath, releaseVersion);
            if (!Directory.Exists(patchesPath))
                return new Patch[0];

            return Directory
                .GetFiles(patchesPath)
                .Select(patch => DeserializeOrThrow<Patch>(patch));
        }

        public void AddPatchToRelease(Release release, Patch patch)
        {
            var releaseDirectory = Path.Combine(Context.Settings.PersistenceDirectoryPath, release.Version);
            var patchPath = Path.Combine(releaseDirectory, patch.Gerrit.Id + ".json");

            // It's possible that the directory doesn't exist yet
            // We'll create if necessary
            if (!Directory.Exists(releaseDirectory))
                Directory.CreateDirectory(releaseDirectory);

            // We shouldn't proceed if the file exists
            // In that case an Update should be performed
            if (File.Exists(patchPath))
                throw new InvalidOperationException($"Patch [{patch.Gerrit.Id}] has already been added to release [{release.Version}]");

            File.WriteAllText(patchPath, SerializeOrThrow(patch, patch.Gerrit.Id));
        }

        public void UpdateReleasePatch(Release release, Patch patch)
        {
            var releaseDirectory = Path.Combine(Context.Settings.PersistenceDirectoryPath, release.Version);
            var patchPath = Path.Combine(releaseDirectory, patch.Gerrit.Id + ".json");

            // We shouldn't proceed if the file doesn't exists
            // In that case an Add should be performed
            if (!File.Exists(patchPath))
                throw new InvalidOperationException($"Unable to update patch [{patch.Gerrit.Id}] for release [{release.Version}], it hasn't been added yet");

            File.WriteAllText(patchPath, SerializeOrThrow(patch, patch.Gerrit.Id));
        }

        private T DeserializeOrThrow<T>(string filePath)
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

        private string SerializeOrThrow<T>(T value, int id)
        {
            try
            {
                return JsonHelper<T>.ObjectToJson(value);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to write json content for patch [{id}]", e);
            }
        }
    }
}
