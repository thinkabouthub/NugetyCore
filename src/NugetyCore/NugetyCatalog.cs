using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace NugetyCore
{
    public class NugetyCatalog : INugetyCatalogProvider
    {
        public static INugetyCatalogProvider Catalog { get; set; }

        private static readonly object _lock = new object();

        private NugetyOptions options;

        public NugetyOptions Options { get
            {
                return options ?? (options =  new NugetyOptions(this));
            }
        }

        public NugetyCatalog() { }

        public virtual T Load<T>(ModuleInfo module)
        {
            var instance = (T)module.AssemblyInfo.Assembly.CreateInstance(module.ModuleInitialiser.FullName);
            return instance;
        }

        public virtual IEnumerable<T> Load<T>(IEnumerable<ModuleInfo> modules)
        {
            var instances = new Collection<T>();
            foreach (var module in modules)
            {
                instances.Add(this.Load<T>(module));
            }
            return instances;
        }

        public virtual Type GetModuleInitializer<T>(Assembly assembly)
        {
            return this.GetModuleInitializer(assembly, typeof(T));
        }

        public virtual Type GetModuleInitializer(Assembly assembly, Type initialiser)
        {
            var type = assembly.GetTypes().FirstOrDefault(t => !t.GetTypeInfo().IsInterface && initialiser.IsAssignableFrom(t));
            if (type != null)
            {
                return type;
            }
            type = assembly.ExportedTypes.FirstOrDefault(t => t == initialiser);
            if (type != null)
            {
                return type;
            }
            return null;
        }

        public virtual IEnumerable<ModuleInfo<T>> GetMany<T>(params Func<INugetyCatalogProvider, IEnumerable<ModuleInfo<T>>>[] loaders)
        {
            var loader = new DirectoryModuleProvider(this);
            var loadModules = new List<ModuleInfo<T>>();

            foreach (var l in loaders)
            {
                loadModules.AddRange(l.Invoke(this));
            }
            return loadModules.AsEnumerable();
        }

        public virtual IDirectoryModuleProvider FromDirectory(string location = "Modules")
        {
           return new DirectoryModuleProvider(this).Options.SetLocation(location);
        }
    }
}
