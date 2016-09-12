using System.Collections.Generic;

namespace Nugety
{
    public class NugetModuleLoader : INugetModuleLoader
    {
        private NugetLoaderOptions options;

        public NugetModuleLoader(INugetyCatalogProvider catalog)
        {
            Catalog = catalog;
        }

        public INugetyCatalogProvider Catalog { get; }

        public NugetLoaderOptions Options
        {
            get { return options ?? (options = new NugetLoaderOptions(this)); }
        }

        public virtual IEnumerable<ModuleInfo<T>> GetModules<T>(params string[] name)
        {
            return null;
        }
    }
}