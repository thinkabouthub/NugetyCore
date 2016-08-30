using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NugetyCore;
using Autofac;

namespace NugetyCore.Tests.Module1
{
    public class ModuleInitializer : Autofac.Module, IModuleInitializer
    {
        public ModuleInitializer()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
        }

        public bool ConfigureServices(IServiceCollection services, IMvcBuilder builder, IServiceProvider provider = null)
        {
            return true;
        }

        public bool Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            return false;
        }
    }
}
