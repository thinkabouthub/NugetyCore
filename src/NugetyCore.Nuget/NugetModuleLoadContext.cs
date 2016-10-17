using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Nugety
{
    public class NugetModuleLoadContext : AssemblyLoadContext
    {
        public NugetModuleLoadContext(INugetModuleLoader loader, DirectoryInfo directory)
        {
            Loader = loader;
            Directory = directory;
        }

        public INugetModuleLoader Loader { get; }

        public INugetyCatalogProvider Catalog => Loader.Catalog; 

        public DirectoryInfo Directory { get; private set; }

        public ModuleInfo ModuleInfo { get; private set; }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }

        public virtual ModuleInfo LoadUsingFileName<T>()
        {
            return null;
        }
    }
}