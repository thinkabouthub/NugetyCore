using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using NugetyCore.Tests.Common;

namespace NugetyCore.Tests.Module2
{
    public class ModuleInitializer : Autofac.Module, IModuleInitializer, IDependency1Version
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

        public Type GetDependency1Type()
        {
            var test = new Dependency1.Class1();
            return test.GetType();
        }
    }
}
