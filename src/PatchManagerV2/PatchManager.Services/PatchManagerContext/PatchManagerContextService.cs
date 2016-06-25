using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PatchManager.Config;

namespace PatchManager.Services.Context
{
    public class PatchManagerContextService : IPatchManagerContextService
    {
        public DateTime Now => DateTime.Now;
        public SettingsConfiguration Settings { get; }

        public PatchManagerContextService(IOptions<SettingsConfiguration> settings)
        {
            Settings = settings.Value;
        }
    }
}
