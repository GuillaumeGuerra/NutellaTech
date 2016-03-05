using System;
using PatchManager.Models;

namespace PatchManager.Services.ModelService
{
    public class GerritWithMetadata
    {
        public Patch Patch { get; set; }

        public DateTime LastRefresh { get; set; }

        public GerritWithMetadata(Patch patch)
        {
            Patch = patch;
            LastRefresh = DateTime.MinValue;
        }
    }
}