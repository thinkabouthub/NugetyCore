using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using System.IO;

namespace NugetyCore
{
    public class DirectoryModuleLoadContext : AssemblyLoadContext
    {
        public IDirectoryModuleProvider Loader { get; private set; }

        public INugetyCatalogProvider Catalog { get { return this.Loader.Catalog; } }

        public DirectoryInfo Directory { get; private set; }

        public ModuleInfo ModuleInfo { get; private set; }

        public DirectoryModuleLoadContext(IDirectoryModuleProvider loader, DirectoryInfo directory)
        {
            this.Loader = loader;
            this.Directory = directory;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Assembly assembly = null;
            var library = DependencyContext.Default.RuntimeLibraries.FirstOrDefault(l => l.GetDefaultAssemblyNames(DependencyContext.Default).Any(a => a.Equals(assemblyName)));
            if (library != null)
            {
                assembly = Assembly.Load(assemblyName); 
            }
            if (assembly == null)
            {
                var file = this.Directory.GetFileSystemInfos(string.Concat(assemblyName.Name, ".dll"), SearchOption.AllDirectories).FirstOrDefault();
                if (file != null)
                {
                    assembly = this.LoadFromAssemblyPath(file.FullName);
                }
            }
            if (assembly == null)
            {
                assembly = Assembly.Load(assemblyName);
            }
            if (assembly != null)
            {
                this.ModuleInfo.AddRelated(new AssemblyInfo(assembly));
            }
            return assembly;
        }

        public virtual ModuleInfo<T> LoadUsingFileName<T>()
        {
            /// TODO: Can this search be optimised?
            foreach (var file in this.Directory.GetFileSystemInfos(!string.IsNullOrEmpty(this.Catalog.Options.FileNameFilterPattern) ? this.Catalog.Options.FileNameFilterPattern : "*.dll", SearchOption.AllDirectories))
            {
                if (!DependencyContext.Default.RuntimeLibraries.Any(l => l.GetDefaultAssemblyNames(DependencyContext.Default).Any(a => a.Name == Path.GetFileNameWithoutExtension(file.Name))))
                {
                    var assembly = this.LoadFromAssemblyPath(file.FullName);
                    this.ModuleInfo = new ModuleInfo<T>(this.Catalog, this.Directory.Name, new AssemblyInfo(assembly));
                    var type = this.Catalog.GetModuleInitializer<T>(assembly);
                    if (type != null)
                    {
                        this.ModuleInfo.AddModuleInitialiser(type);
                        return (ModuleInfo<T>)this.ModuleInfo;
                    }
                }
            }
            return null;
        }
    }
}
