using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nugety;

namespace NugetyCore.Tests.Module
{
    public class ModuleInitializer : Autofac.Module, IModuleInitializer
    {
        public bool ConfigureServices(IServiceCollection services, IMvcBuilder builder, IServiceProvider provider = null)
        {
            return true;
        }

        public bool Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            return false;
        }

        protected override void Load(ContainerBuilder builder)
        {
        }
    }
}
