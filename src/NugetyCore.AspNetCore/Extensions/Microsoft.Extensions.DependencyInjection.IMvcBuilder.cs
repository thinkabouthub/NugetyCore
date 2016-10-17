using System;
using System.Linq;
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

        public static bool ApplicationPartExists(this IMvcBuilder builder, params Type[] types)
        {
            return builder.PartManager.ApplicationParts.OfType<TypesPart>().Any(p => p.Types.Any(i => types.Any(t => t == i.AsType())));
        }

        public static bool ApplicationPartExists<T>(this IMvcBuilder builder) where T : Type
        {
            return builder.PartManager.ApplicationParts.OfType<TypesPart>().Any(p => p.Types.Any(i => i.AsType() == typeof(T)));
        }

        public static IMvcBuilder AddApplicationPart(this IMvcBuilder builder, params Type[] types)
        {
            builder.ConfigureApplicationPartManager(manager => { manager.ApplicationParts.Add(new TypesPart(types)); });
            return builder;
        }

        public static IMvcBuilder AddApplicationPart<T>(this IMvcBuilder builder) where T : Type
        {
            builder.ConfigureApplicationPartManager(manager => { manager.ApplicationParts.Add(new TypesPart(typeof(T))); });
            return builder;
        }

        public static IMvcBuilder RemoveApplicationPart(this IMvcBuilder builder, params Type[] types)
        {
            var parts = builder.PartManager.ApplicationParts.OfType<TypesPart>().Where(p => p.Types.Any(i => types.Any(t => t == i.AsType()))).ToList();
            foreach (var part in parts)
            {
                builder.ConfigureApplicationPartManager(manager => { manager.ApplicationParts.Remove(part); });
            }
            return builder;
        }

        public static IMvcBuilder RemoveApplicationPart<T>(this IMvcBuilder builder) where T : Type
        {
            var parts = builder.PartManager.ApplicationParts.OfType<TypesPart>().Where(p => p.Types.Any(i => i.AsType() == typeof(T))).ToList();
            foreach (var part in parts)
            {
                builder.ConfigureApplicationPartManager(manager => { manager.ApplicationParts.Remove(part); });
            }
            return builder;
        }
    }
}