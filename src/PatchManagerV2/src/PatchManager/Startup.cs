using System;
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
using PatchManager.Services.ModelService;
using PatchManager.Services.PersistenceService;
using PatchManager.Services.GerritService;
using PatchManager.Services.JiraService;
using PatchManager.Services.StatusResolverService;

namespace PatchManager
{
    public class Startup
    {
        public static IContainer Container { get; private set; }
        public static IConfigurationRoot Configuration { get; set; }
        public static SettingsConfiguration Settings { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("services.json")
                .AddJsonFile("settings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Settings = Configuration.Get<SettingsConfiguration>();
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

            builder.Populate(services);
            Container = builder.Build();

            return Container.Resolve<IServiceProvider>();
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

            app.UseDefaultFiles(new DefaultFilesOptions() { DefaultFileNames = new[] { "index.html" } });
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
