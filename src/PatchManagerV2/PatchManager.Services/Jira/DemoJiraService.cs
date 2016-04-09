using System;
using System.Threading;
using PatchManager.Model.Services;
using PatchManager.Models;

namespace PatchManager.Services.Jira
{
    public class DemoJiraService : IJiraService
    {
        public JiraInformation GetJiraInformation(string jiraId)
        {
            var random = new Random((int)DateTime.Now.Ticks);

            Thread.Sleep(random.Next(4) * 1000);

            var values = Enum.GetValues(typeof(JiraStatus));
            var titles = new[]
            {
                "Darth Vader you stink",
                "Palpatine could use some botox injection once in a while",
                "Han, you're dead now, but you got to date Leia, lucky you"
            };
            return new JiraInformation()
            {
                Status = (JiraStatus) values.GetValue(random.Next(values.Length)),
                Description = titles[random.Next(titles.Length)],
            };
        }

        public bool Resolve(string jiraId)
        {
            return true;
        }
    }
}