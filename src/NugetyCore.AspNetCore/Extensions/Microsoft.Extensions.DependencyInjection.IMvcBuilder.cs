using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using NugetyCore;

namespace NugetyCore
{
    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder InitialiseModules(this IMvcBuilder builder, IEnumerable<IModuleInitializer> modules)
        {
            foreach (var m in modules)
            {
                m.ConfigureServices(builder.Services, builder);
            }
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

        public static IMvcBuilder AddApplicationPartByType(this IMvcBuilder builder, params Type[] types)
        {
            builder.ConfigureApplicationPartManager(manager =>
            {
                manager.ApplicationParts.Add(new TypesPart(types));
            });
            return builder;
        }
    }
}
