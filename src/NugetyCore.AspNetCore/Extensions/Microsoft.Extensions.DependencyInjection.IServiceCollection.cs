using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Nugety
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection InitialiseModules(this IServiceCollection services,
            IEnumerable<IModuleInitializer> modules)
        {
            foreach (var m in modules)
                m.ConfigureServices(services);
            return services;
        }
    }
}