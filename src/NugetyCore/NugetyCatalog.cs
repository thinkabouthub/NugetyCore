using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;

namespace Nugety
{
    public class NugetyCatalog : INugetyCatalogProvider
    {
        private static readonly object _lock = new object();

        private NugetyOptions options;

        public static INugetyCatalogProvider Catalog { get; set; }

        public ILoggerFactory Logger { get; set; }

        private readonly Collection<ModuleInfo> _modules = new Collection<ModuleInfo>();

        public NugetyOptions Options => options ?? (options = new NugetyOptions(this));

        public event EventHandler<ModuleIntanceEventArgs> ModuleLoaded;

        public event EventHandler<ModuleCancelEventArgs> ModuleLoading;

        public NugetyCatalog()
        {
            AssemblyLoadContext.Default.Resolving += this.OnAssemblyResolve;
        }

        public IEnumerable<ModuleInfo> Modules => this._modules;

        public void AddModule(ModuleInfo module)
        {
            lock (_lock)
            {
                this._modules.Add(module);
            }
        }

        public void RemoveModule(ModuleInfo module)
        {
            lock (_lock)
            {
                this._modules.Remove(module);
            }
        }

        public event EventHandler<AssemblyResolveCancelEventArgs> AssemblyResolve;

        public event EventHandler<AssemblyResolvedEventArgs> AssemblyResolved;

        protected void OnAssemblyResolved(AssemblyResolvedEventArgs args)
        {
            this.AssemblyResolved?.Invoke(this, args);
        }

        protected void OnAssemblyResolve(AssemblyResolveCancelEventArgs args)
        {
            this.AssemblyResolve?.Invoke(this, args);
        }

        private readonly Collection<AssemblyName> assemblyResolveFailed = new Collection<AssemblyName>();

        public IEnumerable<AssemblyName> AssemblyResolveFailed => this.assemblyResolveFailed;

        protected virtual Assembly OnAssemblyResolve(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
        {
            return null;
            // return assemblyLoadContext.LoadFromAssemblyName(assemblyName) ?? this.ResolveAssembly(assemblyName);
        }

        public virtual Assembly ResolveAssembly(AssemblyName name)
        {
            lock (_lock)
            {
                var cancelArgs = new AssemblyResolveCancelEventArgs(name);
                this.OnAssemblyResolve(cancelArgs);
                if (!cancelArgs.Cancel)
                {
                    if (!this.AssemblyResolveFailed.Any(n => n.Name.Equals(name.Name)))
                    {
                        var assemblies =
                            this.Modules.SelectMany(
                                m => m.Assemblies.Where(d => d.Assembly.GetName().Name.Equals(name.Name))).ToList();
                        if (assemblies.Any()) return assemblies.First().Assembly;

                        foreach (var module in this.Modules.Where(m => m.AllowAssemblyResolve))
                        {
                            var assemblyInfo = module.ResolveAssembly(name);
                            if (assemblyInfo != null)
                            {
                                var args = new AssemblyResolvedEventArgs(name, module, assemblyInfo);
                                this.OnAssemblyResolved(args);
                                return assemblyInfo.Assembly;
                            }
                        }
                        this.assemblyResolveFailed.Add(name);
                    }
                    return null;
                }
                return cancelArgs.Assembly;
            }
        }

        public virtual T Load<T>(ModuleInfo module)
        {
            var args = new ModuleCancelEventArgs(module);
            OnModuleLoading(args);
            if (!args.Cancel)
            {
                var instance = (T) module.AssemblyInfo.Assembly.CreateInstance(module.ModuleInitialiser.FullName);
                OnModuleLoaded(module, instance);
                return instance;
            }
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
            try
            {
                var type = assembly.GetTypes().FirstOrDefault(t => !t.GetTypeInfo().IsInterface && initialiser.IsAssignableFrom(t));
                if (type != null) return type;
                type = assembly.ExportedTypes.FirstOrDefault(t => t == initialiser);
                if (type != null) return type;
            }
            catch (ReflectionTypeLoadException)
            {
                if (!this.Options.IgnoreLoaderExceptions) throw;
            }
            return null;
        }

        public virtual IEnumerable<ModuleInfo<T>> GetMany<T>(
            params Func<INugetyCatalogProvider, IEnumerable<ModuleInfo<T>>>[] loaders)
        {
            var loadModules = new List<ModuleInfo<T>>();

            foreach (var l in loaders) loadModules.AddRange(l.Invoke(this));
            return loadModules.AsEnumerable();
        }

        public virtual IDirectoryModuleProvider FromDirectory(string location = "Nugety")
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

        public virtual INugetyCatalogProvider UseLoggerFactory(ILoggerFactory loggerFactory)
        {
            this.Logger = loggerFactory;
            return this;
        }
    }
}