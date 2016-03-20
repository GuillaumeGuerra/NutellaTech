﻿using System;
using System.Reflection;
using Autofac;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PatchManager.Config;
using Autofac.Extensions.DependencyInjection;
using PatchManager.Model.Services;
using PatchManager.Services.PatchActions;

namespace PatchManager
{
    public class Startup
    {
        //TODO : update the model when the status resolver decided to refresh jira and gerrit status
        //TODO : add logs
        //TODO : find a way to log all exceptions in a single location
        //TODO : implement actual gerrit and jira services
        //TODO : provide a grid view in addition to the card one
        //TODO : show the refresh and refresh all buttons in a 3 points menu on the top right corner of the cards
        //DONE : review the "Register gerrit" form using material api
        //TODO : handle authentication, to use RM credentials in Gerrit and Jira
        //TODO : split the api into a dedicated project without DNX, and write proper UTs
        //TODO : think about what can be threaded, to perform asynchronous save into the model
        //TODO : handle exceptions in the website, and show a popup with the error message
        //TODO : show a progress bar when background actions are ongoing, freeze specific buttons that can't be run in the same time
        //TODO : provide multiple views in the client : "gerrit gathering" (no other status than accepted or not), "merge" (filter non accepted patches, show only merge and jira status), "test" (only test status)
        //TODO : think about a way to provide a unique "meta-status" : "gerrit to accept", "gerrit to merge and jira to review", "gerrit to test". it should be a combination of all status into a single meaningful one
        //TODO : use the proper button class for the search button in the add-gerrit page
        //DONE : add the owner of the gerrit (the one who who pushes the mail), in addition to the actual author of the gerrit, that can be off for instance
        //TODO : use tabs in the gerrit addtion page, to separate gerrit and jira informations
        //DONE : add the team asset for each gerrit 
        //TODO : provide a quick searh feature
        //DONE : refactor the names : release => patch => user inputs + gerrit api data + jira api data
        //TODO : create features usable by RM only, such as release creation
        //TODO : give a quick switch to define which description to show in the cards : jira or gerrit
        //TOO : write at least a simple implementation for the persistence service, using json files for instance


        public static IContainer Container { get; private set; }
        public static IConfigurationRoot Configuration { get; set; }
        //public static SettingsConfiguration Settings { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("services.json")
                .AddJsonFile("settings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            SettingsConfiguration.Settings = Configuration.Get<SettingsConfiguration>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvcCore().
                AddJsonFormatters(a => a.ContractResolver = new CamelCasePropertyNamesContractResolver()).
                AddJsonFormatters(a => a.Converters.Add(new StringEnumConverter()));

            var conf = Configuration.Get<ServicesConfiguration>();

            var builder = new ContainerBuilder();

            RegisterCustomService<IPersistenceService>(builder, conf.Persistence);
            RegisterCustomService<IModelService>(builder, conf.Model);
            RegisterCustomService<IGerritService>(builder, conf.Gerrit);
            RegisterCustomService<IJiraService>(builder, conf.Jira);
            RegisterCustomService<IStatusResolverService>(builder, conf.Resolver);

            RegisterActions(builder);

            builder.Populate(services);
            Container = builder.Build();

            return Container.Resolve<IServiceProvider>();
        }

        private void RegisterActions(ContainerBuilder builder)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterface(typeof(IPatchAction).Name) != null && !type.IsAbstract)
                {
                    var attribute = type.GetCustomAttribute<PatchActionAttribute>();
                    if (attribute == null)
                        throw new InvalidOperationException(string.Format("Missing GerritAction attribute on type [{0}]", type.Name));

                    builder.RegisterType(type).Named<IPatchAction>(attribute.Name.ToUpper());
                }
            }
        }

        private void RegisterCustomService<TService>(ContainerBuilder builder, SingleServiceConfiguration service)
        {
            if (service == null)
                throw new InvalidOperationException(string.Format("Missing configuration for service [{0}] in services.json file", typeof(TService).Name));

            var type = Type.GetType(service.Type);

            if (type == null)
                throw new InvalidOperationException(string.Format("Unknown service type [{0}] for interface [{1}]", service.Type, typeof(TService)));

            var registration = builder.RegisterType(type).As<TService>().PropertiesAutowired();
            if (service.IsSingleton)
                registration.SingleInstance();
            else
                registration.InstancePerDependency();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseDefaultFiles(new DefaultFilesOptions() { DefaultFileNames = new[] { "Material.html" } });
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
