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
        public static IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.

            services.AddMvcCore().
                AddJsonFormatters(a => a.ContractResolver = new CamelCasePropertyNamesContractResolver()).
                AddJsonFormatters(a => a.Converters.Add(new StringEnumConverter()));

            services.Configure<ServicesConfiguration>(Configuration.GetSection("Services"));
            services.Configure<SettingsConfiguration>(Configuration.GetSection("Settings"));

            // Damned it's ugly, I don't know how the get the configuration another way in that particular method, since IOptions<> is not yet available :(
            var servicesConfiguration = new ServicesConfiguration();
            Configuration.GetSection("Services").Bind(servicesConfiguration);

            var builder = new ContainerBuilder();

            builder.RegisterType(typeof(PatchManagerContextService)).As<IPatchManagerContextService>().SingleInstance();

            RegisterCustomService<IPersistenceService>(builder, servicesConfiguration.Persistence);
            RegisterCustomService<IModelService>(builder, servicesConfiguration.Model);
            RegisterCustomService<IGerritService>(builder, servicesConfiguration.Gerrit);
            RegisterCustomService<IJiraService>(builder, servicesConfiguration.Jira);
            RegisterCustomService<IStatusResolverService>(builder, servicesConfiguration.Resolver);

            RegisterActions(builder);

            builder.Populate(services);
            Container = builder.Build();

            return Container.Resolve<IServiceProvider>();
        }

        private void RegisterActions(ContainerBuilder builder)
        {
            foreach (var type in typeof(PatchActionAttribute).Assembly.GetTypes())
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
    }
}
