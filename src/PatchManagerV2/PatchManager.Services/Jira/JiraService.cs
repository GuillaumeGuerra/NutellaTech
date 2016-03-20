using System;
using System.Net.Http;
using PatchManager.Config;
using PatchManager.Model.Services;
using PatchManager.Models;

namespace PatchManager.Services.Jira
{
    public class JiraService : IJiraService
    {
        private const string GetJiraUrl = "rest/api/2/issue/{jiraId}";

        public JiraInformation GetJiraInformation(string jiraId)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(SettingsConfiguration.Settings.JiraUrl);
            var response = client.GetAsync(FormatGetJiraQuery(jiraId)).Result.Content.ReadAsAsync<Rootobject>().Result;

            return new JiraInformation()
            {
                Description = response.fields.description,
                Id = jiraId,
                Status = ParseStatus(response.fields.status.name)
            };
        }

        public bool Resolve(string jiraId)
        {
            throw new NotImplementedException("TODO !!!!");
        }

        private JiraStatus ParseStatus(string status)
        {
            switch (status)
            {
                case "In Progress":
                    return JiraStatus.InProgress;
                case "Resolved":
                    return JiraStatus.Resolved;
                case "Closed":
                    return JiraStatus.Closed;
                case "Open":
                    return JiraStatus.Open;
                case "Approved":
                    return JiraStatus.Approved;
                default:
                    return JiraStatus.Unknown;
            }
        }

        private string FormatGetJiraQuery(string jiraId)
        {
            return GetJiraUrl.Replace("{jiraId}", jiraId);
        }

        public class Rootobject
        {
            public string id { get; set; }
            public Fields fields { get; set; }
        }

        public class Fields
        {
            public string description { get; set; }
            public Project project { get; set; }
            public Comment[] comment { get; set; }
            public Status status { get; set; }
        }

        public class Project
        {
            public string self { get; set; }
            public string id { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public Projectcategory projectCategory { get; set; }
        }

        public class Projectcategory
        {
            public string self { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class Comment
        {
            public string self { get; set; }
            public string id { get; set; }
            public Author author { get; set; }
            public string body { get; set; }
            public Updateauthor updateAuthor { get; set; }
            public DateTime created { get; set; }
            public DateTime updated { get; set; }
        }

        public class Author
        {
            public string self { get; set; }
            public string name { get; set; }
            public string displayName { get; set; }
            public bool active { get; set; }
        }

        public class Updateauthor
        {
            public string self { get; set; }
            public string name { get; set; }
            public string displayName { get; set; }
            public bool active { get; set; }
        }

        public class Status
        {
            public int id { get; set; }
            public string name { get; set; }
        }
    }
}