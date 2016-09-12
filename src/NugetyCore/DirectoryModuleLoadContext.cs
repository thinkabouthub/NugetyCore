using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

namespace Nugety
{
    public class DirectoryModuleLoadContext : AssemblyLoadContext
    {
        public DirectoryModuleLoadContext(IDirectoryModuleProvider loader, DirectoryInfo directory)
        {
            Loader = loader;
            Directory = directory;
        }

        public IDirectoryModuleProvider Loader { get; }

        public INugetyCatalogProvider Catalog
        {
            get { return Loader.Catalog; }
        }

        public DirectoryInfo Directory { get; }

        public ModuleInfo ModuleInfo { get; private set; }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Assembly assembly = null;
            var library =
                DependencyContext.Default.RuntimeLibraries.FirstOrDefault(
                    l => l.GetDefaultAssemblyNames(DependencyContext.Default).Any(a => a.Equals(assemblyName)));
            if (library != null)
                assembly = Assembly.Load(assemblyName);
            if (assembly == null)
            {
                var file =
                    Directory.GetFileSystemInfos(string.Concat(assemblyName.Name, ".dll"), SearchOption.AllDirectories)
                        .FirstOrDefault();
                if (file != null)
                    assembly = LoadFromAssemblyPath(file.FullName);
            }
            if (assembly == null)
                assembly = Assembly.Load(assemblyName);
            if (assembly != null)
                ModuleInfo.AddRelated(new AssemblyInfo(assembly));
            return assembly;
        }

        public virtual ModuleInfo<T> LoadUsingFileName<T>()
        {
            /// TODO: Can this search be optimised?
            foreach (
                var file in
                Directory.GetFileSystemInfos(
                    !string.IsNullOrEmpty(Catalog.Options.FileNameFilterPattern)
                        ? Catalog.Options.FileNameFilterPattern
                        : "*.dll", SearchOption.AllDirectories))
                if (
                    !DependencyContext.Default.RuntimeLibraries.Any(
                        l =>
                            l.GetDefaultAssemblyNames(DependencyContext.Default)
                                .Any(a => a.Name == Path.GetFileNameWithoutExtension(file.Name))))
                {
                    var assembly = LoadFromAssemblyPath(file.FullName);
                    ModuleInfo = new ModuleInfo<T>(Catalog, Directory.Name, new AssemblyInfo(assembly));
                    var type = Catalog.GetModuleInitializer<T>(assembly);
                    if (type != null)
                    {
                        ModuleInfo.AddModuleInitialiser(type);
                        return (ModuleInfo<T>) ModuleInfo;
                    }
                }
            return null;
        }
    }
}