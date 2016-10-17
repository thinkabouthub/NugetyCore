using System;
using System.Configuration;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nugety.Tests.Module5
{
    public class ModuleInitializer : Autofac.Module, IModuleInitializer
    {
        public bool ConfigureServices(IServiceCollection services, IMvcBuilder builder, IServiceProvider provider = null)
        {
            var test = ConfigurationManager.GetSection("unity");
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