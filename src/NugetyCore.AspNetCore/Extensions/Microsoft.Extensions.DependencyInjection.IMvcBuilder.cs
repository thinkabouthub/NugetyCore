using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Nugety
{
    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder InitialiseModules(this IMvcBuilder builder, IEnumerable<IModuleInitializer> modules)
        {
            foreach (var m in modules)
                m.ConfigureServices(builder.Services, builder);
            return builder;
        }

        public static IMvcBuilder InitialiseModules(this IMvcBuilder builder, IEnumerable<ModuleInfo> modules)
        {
            foreach (var m in modules)
            {
                var initializer = m.Catalog.Load<IModuleInitializer>(m);
                initializer.ConfigureServices(builder.Services, builder);
            }
            return builder;
        }

        public static IMvcBuilder InitialiseModules(this IMvcBuilder builder, string fileNameFilterPattern,
            params string[] moduleName)
        {
            var modules = new NugetyCatalog()
                .Options.SetModuleFileNameFilter(fileNameFilterPattern)
                .FromDirectory()
                .GetModules<IModuleInitializer>(moduleName).Load();
            builder.InitialiseModules(modules);

            return builder;
        }

        public static IMvcBuilder InitialiseModules(this IMvcBuilder builder, params string[] moduleName)
        {
            var modules = new NugetyCatalog()
                .FromDirectory()
                .GetModules<IModuleInitializer>(moduleName).Load();
            builder.InitialiseModules(modules);

            return builder;
        }

        public static IMvcBuilder AddApplicationPartByType(this IMvcBuilder builder, params Type[] types)
        {
            builder.ConfigureApplicationPartManager(manager => { manager.ApplicationParts.Add(new TypesPart(types)); });
            return builder;
        }
    }
}