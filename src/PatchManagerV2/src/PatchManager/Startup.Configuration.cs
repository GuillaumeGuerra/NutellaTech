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
using NuGet.Packaging;

namespace PatchManager
{
    public partial class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<SettingsConfiguration> settings)
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

            app.UseDefaultFiles(new DefaultFilesOptions() { DefaultFileNames = new[] { "Material.html" } });
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var url = app.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
            url.Clear();
            url.AddRange(settings.Value.PatchManagerUrl);
        }
    }
}
