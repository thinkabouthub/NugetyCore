using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

namespace Nugety
{
    public class DirectoryModuleInfo<T> : ModuleInfo<T>
    {
        public DirectoryModuleInfo(IModuleProvider provider, DirectoryInfo directory) 
            : base(provider, directory.Name)
        {
            this.Directory = directory;
            this.Context = new DirectoryModuleLoadContext(provider, this, directory);
        }

        public DirectoryInfo Directory { get; }

        public DirectoryModuleLoadContext Context { get; }

        public override ModuleInfo LoadModule(Type moduleType)
        {
            foreach (var file in this.Directory.GetFileSystemInfos(
                    !string.IsNullOrEmpty(Catalog.Options.ModuleFileNameFilterPattern)
                        ? Catalog.Options.ModuleFileNameFilterPattern
                        : "*.dll", SearchOption.AllDirectories))
            {
                if (!DependencyContext.Default.RuntimeLibraries.Any(l => l.GetDefaultAssemblyNames(DependencyContext.Default).Any(a => a.Name == Path.GetFileNameWithoutExtension(file.Name))))
                {
                    var assembly = this.Context.LoadFromAssemblyPath(file.FullName);
                    if (assembly != null)
                    {
                        this.AssemblyInfo = new AssemblyInfo(assembly);
                        this.ModuleInitialiser = this.Catalog.GetModuleInitializer(assembly, moduleType);
                    }
                }
            }
            return null;
        }

        public override AssemblyInfo LoadAssembly(string location, AssemblyName name = null)
        {
            var assembly = this.Context.LoadFromAssemblyPath(location);
            return assembly != null ? new AssemblyInfo(assembly) : null;
        }

        public override AssemblyInfo ResolveAssembly(AssemblyName name)
        {
            var directory = new DirectoryInfo(Path.GetDirectoryName(this.Location));
            var filtered = directory.GetFileSystemInfos(string.Concat(name.Name, ".dll"), SearchOption.AllDirectories);
            var assemblyInfo = this.ResolveAssembly(name, filtered);

            if (assemblyInfo == null)
            {
                var files = directory.GetFileSystemInfos("*.dll", SearchOption.AllDirectories).Where(f => !filtered.Any(t => t.Name.Equals(f.Name))).ToArray();
                assemblyInfo = this.ResolveAssembly(name, files);
            }
            if (assemblyInfo != null)
            {
                this.AddAssembly(assemblyInfo);
            }
            return assemblyInfo;
        }

        protected virtual AssemblyInfo ResolveAssembly(AssemblyName name, FileSystemInfo[] files)
        {
            if (files.Any())
            {
                var libraries = DependencyContext.Default.RuntimeLibraries;
                foreach (var file in files)
                {
                    var assemblyName = AssemblyLoadContext.GetAssemblyName(file.FullName);
                    if (!libraries.Any(l => this.IsCandidateLibrary(l, name)))
                    {
                        try
                        {
                            var info = this.LoadAssembly(file.FullName, assemblyName);
                            if (info != null) return info;
                        }
                        catch
                        {
                            // Consume exception. Assembly failing to load should not fail all assemblies.
                        }
                    }
                }
            }
            return null;
        }

        protected virtual bool IsCandidateLibrary(RuntimeLibrary library, AssemblyName name)
        {
            return library.Name == (name.Name) || library.Dependencies.Any(d => d.Name.StartsWith(name.Name));
        }
    }

    public abstract class ModuleInfo<T> : ModuleInfo
    {
        protected ModuleInfo(IModuleProvider provider, string name) : base(provider, name)
        {
        }

    }

    public abstract class ModuleInfo
    {
        public static readonly object _lock = new object();

        private readonly Collection<AssemblyInfo> _assemblies = new Collection<AssemblyInfo>();

        protected ModuleInfo(IModuleProvider provider, string name)
        {
            this.ModuleProvider = provider;
            this.Name = name;
            this.AllowAssemblyResolve = true;
        }


        public virtual INugetyCatalogProvider Catalog => this.ModuleProvider?.Catalog;

        public virtual IModuleProvider ModuleProvider { get; protected set; }

        public virtual Type ModuleInitialiser { get; protected set; }

        public virtual bool AllowAssemblyResolve { get; set; }

        public virtual string Name { get; protected set; }

        public virtual string Location => AssemblyInfo.Assembly.Location; 

        public virtual AssemblyInfo AssemblyInfo { get; protected set; }

        public virtual IEnumerable<AssemblyInfo> Assemblies => this._assemblies;

        protected virtual void AddModuleInitialiser(Type type)
        {
            this.ModuleInitialiser = type;
            var assembly = type.GetTypeInfo().Assembly;
            this.AddAssembly(new AssemblyInfo(assembly));
        }

        public virtual void AddAssembly(AssemblyInfo info)
        {
            lock (_lock)
            {
                if (!this.Assemblies.Any(a => a.Assembly.GetName().Equals(info.Assembly.GetName())))
                {
                    this._assemblies.Add(info);
                }
            }
        }

        public virtual void RemoveAssembly(AssemblyInfo info)
        {
            lock (_lock)
            {
                if (!this.Assemblies.Any(a => a.Assembly.GetName().Equals(info.Assembly.GetName())))
                {
                    this._assemblies.Remove(info);
                }
            }
        }

        public virtual ModuleInfo LoadModule<T>()
        {
            return this.LoadModule(typeof(T));
        }

        public abstract ModuleInfo LoadModule(Type moduleType);

        public abstract AssemblyInfo LoadAssembly(string location, AssemblyName name = null);

        public abstract AssemblyInfo ResolveAssembly(AssemblyName name);

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Name) ? Name : base.ToString();
        }
    }
}