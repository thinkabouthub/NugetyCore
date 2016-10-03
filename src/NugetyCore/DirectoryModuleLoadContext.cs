using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

namespace Nugety
{
    public class DirectoryModuleLoadContext : AssemblyLoadContext
    {
        public DirectoryModuleLoadContext(IModuleProvider provider, DirectoryInfo directory)
        {
            this.Provider = provider;
            this.Directory = directory;
        }

        public IModuleProvider Provider { get; private set; }

        public DirectoryInfo Directory { get; }

        public ModuleInfo ModuleInfo { get; set; }

        public INugetyCatalogProvider Catalog { get { return this.Provider.Catalog; } }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Assembly assembly = null;
            var library = DependencyContext.Default.RuntimeLibraries.FirstOrDefault(l => l.GetDefaultAssemblyNames(DependencyContext.Default).Any(a => a.Equals(assemblyName)));
            if (library != null) assembly = Assembly.Load(assemblyName);
            if (assembly == null)
            {
                var file = Directory.GetFileSystemInfos(string.Concat(assemblyName.Name, ".dll"), SearchOption.AllDirectories).FirstOrDefault();
                if (file != null) assembly = this.LoadFromAssemblyPath(file.FullName);
            }
            if (assembly == null) assembly = Assembly.Load(assemblyName);
            if (assembly != null) this.ModuleInfo.AddAssembly(new AssemblyInfo(assembly));
            return assembly;
        }

        public virtual ModuleInfo<T> LoadModule<T>()
        {
            foreach (var file in this.Directory.GetFileSystemInfos(
                    !string.IsNullOrEmpty(Catalog.Options.ModuleFileNameFilterPattern)
                        ? Catalog.Options.ModuleFileNameFilterPattern
                        : "*.dll", SearchOption.AllDirectories))
            {
                if (!DependencyContext.Default.RuntimeLibraries.Any(l => l.GetDefaultAssemblyNames(DependencyContext.Default).Any(a => a.Name == Path.GetFileNameWithoutExtension(file.Name))))
                {
                    var assembly = this.LoadFromAssemblyPath(file.FullName);
                    var type = this.Catalog.GetModuleInitializer<T>(assembly);
                    if (type != null)
                    {
                        this.ModuleInfo = new ModuleInfo<T>(this, this.Provider, this.Directory.Name, new AssemblyInfo(assembly), type);
                        return (ModuleInfo<T>)this.ModuleInfo;
                    }
                }
            }
            return null;
        }
    }
}