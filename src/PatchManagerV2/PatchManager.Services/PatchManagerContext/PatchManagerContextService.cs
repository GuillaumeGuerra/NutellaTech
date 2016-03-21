using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatchManager.Config;

namespace PatchManager.Services.Context
{
    public class PatchManagerContextService : IPatchManagerContextService
    {
        public DateTime Now => DateTime.Now;
        public SettingsConfiguration Settings { get; private set; }

        public PatchManagerContextService(SettingsConfiguration settings)
        {
            Settings = settings;
        }
    }
}
