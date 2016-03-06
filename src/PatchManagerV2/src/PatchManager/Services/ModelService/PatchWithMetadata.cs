using System;
using PatchManager.Models;

namespace PatchManager.Services.ModelService
{
    public class PatchWithMetadata
    {
        public Patch Patch { get; set; }

        public DateTime LastRefresh { get; set; }

        public PatchWithMetadata(Patch patch)
        {
            Patch = patch;
            LastRefresh = DateTime.MinValue;
        }
    }
}