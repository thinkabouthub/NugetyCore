using System.Collections.Generic;
using System.Reflection;

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

        public NugetLoaderOptions Options => options ?? (options = new NugetLoaderOptions(this)); 

        public virtual IEnumerable<ModuleInfo<T>> GetModules<T>(params string[] name)
        {
            return null;
        }

        public virtual AssemblyInfo ResolveAssembly(ModuleInfo module, AssemblyName name)
        {
            return null;
        }

        public virtual AssemblyInfo LoadAssembly(ModuleInfo module, string location, AssemblyName name = null)
        {
            return null;
        }
    }
}