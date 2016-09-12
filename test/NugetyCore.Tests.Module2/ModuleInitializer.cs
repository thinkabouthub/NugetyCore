using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nugety.Tests.Common;
using NugetyCore;

namespace Nugety.Tests.Module2
{
    public class ModuleInitializer : Module, IModuleInitializer, IDependency1Version
    {
        public Type GetDependency1Type()
        {
            var test = new Dependency1.Class1();
            return test.GetType();
        }

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