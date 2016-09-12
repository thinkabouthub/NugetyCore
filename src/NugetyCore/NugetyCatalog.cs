using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Nugety
{
    public class NugetyCatalog : INugetyCatalogProvider
    {
        private static readonly object _lock = new object();

        private NugetyOptions options;

        public static INugetyCatalogProvider Catalog { get; set; }

        public NugetyOptions Options
        {
            get { return options ?? (options = new NugetyOptions(this)); }
        }

        public event EventHandler<ModuleIntanceEventArgs> ModuleLoaded;

        public event EventHandler<ModuleCancelEventArgs> ModuleLoading;

        public virtual T Load<T>(ModuleInfo module)
        {
            var args = new ModuleCancelEventArgs(module);
            OnModuleLoading(args);
            //if (!args.Cancel)
            //{
                var instance = (T) module.AssemblyInfo.Assembly.CreateInstance(module.ModuleInitialiser.FullName);
                OnModuleLoaded(module, instance);
                return instance;
            //}
            return default(T);
        }

        public virtual IEnumerable<T> Load<T>(IEnumerable<ModuleInfo> modules)
        {
            var instances = new Collection<T>();
            foreach (var module in modules)
            {
                var i = Load<T>(module);
                if (i != null) instances.Add(i);
            }
            return instances;
        }

        public virtual Type GetModuleInitializer<T>(Assembly assembly)
        {
            return GetModuleInitializer(assembly, typeof(T));
        }

        public virtual Type GetModuleInitializer(Assembly assembly, Type initialiser)
        {
            var type = assembly.GetTypes().FirstOrDefault(t => !t.GetTypeInfo().IsInterface && initialiser.IsAssignableFrom(t));
            if (type != null)
                return type;
            type = assembly.ExportedTypes.FirstOrDefault(t => t == initialiser);
            if (type != null)
                return type;
            return null;
        }

        public virtual IEnumerable<ModuleInfo<T>> GetMany<T>(
            params Func<INugetyCatalogProvider, IEnumerable<ModuleInfo<T>>>[] loaders)
        {
            var loader = new DirectoryModuleProvider(this);
            var loadModules = new List<ModuleInfo<T>>();

            foreach (var l in loaders)
                loadModules.AddRange(l.Invoke(this));
            return loadModules.AsEnumerable();
        }

        public virtual IDirectoryModuleProvider FromDirectory(string location = "Modules")
        {
            return new DirectoryModuleProvider(this).Options.SetLocation(location);
        }

        protected void OnModuleLoaded(ModuleInfo module, object value)
        {
            ModuleLoaded?.Invoke(this, new ModuleIntanceEventArgs(module, value));
        }

        protected void OnModuleLoading(ModuleCancelEventArgs args)
        {
            ModuleLoading?.Invoke(this, args);
        }
    }
}