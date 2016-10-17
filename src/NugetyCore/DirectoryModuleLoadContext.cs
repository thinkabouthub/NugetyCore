using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

namespace Nugety
{
    public class DirectoryModuleLoadContext : AssemblyLoadContext
    {
        public DirectoryModuleLoadContext(IModuleProvider provider, ModuleInfo module, DirectoryInfo directory)
        {
            this.Provider = provider;
            this.Module = module;
            this.Directory = directory;
        }

        public IModuleProvider Provider { get; private set; }

        public DirectoryInfo Directory { get; }

        public ModuleInfo Module { get; set; }

        public INugetyCatalogProvider Catalog => this.Provider.Catalog; 

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Assembly assembly = null;
            var library = DependencyContext.Default.RuntimeLibraries.FirstOrDefault(l => l.GetDefaultAssemblyNames(DependencyContext.Default).Any(a => a.Equals(assemblyName)));
            if (library != null) assembly = Assembly.Load(assemblyName);
            if (assembly == null)
            {
                var file = this.Directory.GetFileSystemInfos(string.Concat(assemblyName.Name, ".dll"), SearchOption.AllDirectories).FirstOrDefault();
                if (file != null) assembly = this.LoadFromAssemblyPath(file.FullName);
            }
            if (assembly == null) assembly = Assembly.Load(assemblyName);
            if (assembly != null) this.Module.AddAssembly(new AssemblyInfo(assembly));
            return assembly;
        }
    }
}