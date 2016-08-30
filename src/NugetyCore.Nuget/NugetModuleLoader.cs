using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Loader;
using System.IO;
using Microsoft.Extensions.DependencyModel;

namespace NugetyCore
{
    public class NugetModuleLoader : INugetModuleLoader
    {
        public INugetyCatalogProvider Catalog { get; private set; }
        public NugetModuleLoader(INugetyCatalogProvider catalog)
        {
            this.Catalog = catalog;
        }

        private NugetLoaderOptions options;

        public NugetLoaderOptions Options
        {
            get
            {
                return options ?? (options = new NugetLoaderOptions(this));
            }
        }

        public virtual IEnumerable<ModuleInfo<T>> GetModules<T>(params string[] name)
        {
            return null;
        }
    }
}
