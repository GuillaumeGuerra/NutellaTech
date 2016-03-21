using System;
using PatchManager.Config;

namespace PatchManager.Services.Context
{
    public interface IPatchManagerContextService
    {
        DateTime Now { get; }
        SettingsConfiguration Settings { get; }
    }
}