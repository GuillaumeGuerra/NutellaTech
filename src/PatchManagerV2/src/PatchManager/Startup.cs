using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PatchManager.Config;
using PatchManager.Model.Services;
using PatchManager.Services.Context;
using PatchManager.Services.PatchActions;
using System.Linq;
using Microsoft.Extensions.Options;

namespace PatchManager
{
    public partial class Startup
    {
        //TODO : add logs
        //TODO : find a way to log all exceptions in a single location
        //TODO : implement actual gerrit and jira services
        //IN PROGRESS : provide a grid view in addition to the card one
        //TODO : handle authentication, to use RM credentials in Gerrit and Jira
        //TODO : handle exceptions in the website, and show a popup with the error message
        //TODO : think about a way to provide a unique "meta-status" : "gerrit to accept", "gerrit to merge and jira to review", "gerrit to test". it should be a combination of all status into a single meaningful one
        //TODO : use tabs in the gerrit addition page, to separate gerrit and jira informations
        //TODO : provide a quick search feature
        //TODO : create features usable by RM only, such as release creation
        //TODO : give a quick switch to define which description to show in the cards : jira or gerrit
        //TODO : be able to open an existing patch in the new patch window, to change the owner and the asset of the patch
        //TODO : provide angular intellisense using typescript package
        //TODO : be able to delete a patch

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("services.json")
                .AddJsonFile("settings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
    }
}
