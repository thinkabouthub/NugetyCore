using System.Collections.Generic;
using Autofac;

namespace Nugety
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterModules(this ContainerBuilder builder, IEnumerable<Module> modules)
        {
            foreach (var m in modules)
                builder.RegisterModule(m);
        }
    }
}