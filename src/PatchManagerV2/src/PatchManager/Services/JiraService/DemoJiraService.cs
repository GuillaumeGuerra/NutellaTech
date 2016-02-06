using System;
using PatchManager.Models;

namespace PatchManager.Services.JiraService
{
    public class DemoJiraService : IJiraService
    {
        public JiraInformation GetJiraInformation(string jiraId)
        {
            var values = Enum.GetValues(typeof(JiraStatus));
            return new JiraInformation()
            {
                Status = (JiraStatus) values.GetValue(new Random((int)DateTime.Now.Ticks).Next(values.Length))
            };
        }
    }
}