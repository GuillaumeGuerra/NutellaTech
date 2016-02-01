using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatchManager.Services.JiraService
{
    public interface IJiraService
    {
        JiraMetadata GetJiraMetadata(string jiraId);
    }
}
