using System;
using PatchManager.Models;

namespace PatchManager.Services.JiraService
{
    public class DemoJiraService : IJiraService
    {
        public JiraMetadata GetJiraMetadata(string jiraId)
        {
            var values = Enum.GetValues(typeof(JiraStatus));
            return new JiraMetadata()
            {
                Status = (JiraStatus) values.GetValue(new Random((int)DateTime.Now.Ticks).Next(values.Length))
            };
        }
    }
}