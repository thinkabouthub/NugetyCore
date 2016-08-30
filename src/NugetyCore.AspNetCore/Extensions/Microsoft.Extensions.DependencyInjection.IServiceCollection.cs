using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NugetyCore
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection InitialiseModules(this IServiceCollection services, IEnumerable<IModuleInitializer> modules)
        {
            foreach (var m in modules)
            {
                m.ConfigureServices(services);
            }
            return services;
        }
    }
}
