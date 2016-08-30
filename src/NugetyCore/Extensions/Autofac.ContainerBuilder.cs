using System.Collections.Generic;
using Autofac;

namespace NugetyCore
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterModules(this ContainerBuilder builder, IEnumerable<Autofac.Module> modules)
        {
            foreach (var m in modules)
            {
                builder.RegisterModule(m);
            }
        }
    }
}
