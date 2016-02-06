using System;
using PatchManager.Models;

namespace PatchManager.Services.ModelService
{
    public class GerritWithMetadata
    {
        public Gerrit Gerrit { get; set; }

        public DateTime LastRefresh { get; set; }

        public GerritWithMetadata(Gerrit gerrit)
        {
            Gerrit = gerrit;
            LastRefresh = DateTime.MinValue;
        }
    }
}