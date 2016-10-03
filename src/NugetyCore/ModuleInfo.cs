using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Nugety
{
    public class ModuleInfo<T> : ModuleInfo
    {
        public ModuleInfo(AssemblyLoadContext context, IModuleProvider provider, string name, AssemblyInfo assemblyInfo, Type moduleInitialiser = null) 
            : base(context, provider, name, assemblyInfo, moduleInitialiser)
        {
        }

        public ModuleInfo(AssemblyLoadContext context, IModuleProvider provider, AssemblyInfo assembly, Type moduleInitialiser = null)
            : base(context, provider, assembly, moduleInitialiser)
        {
        }
    }

    public class ModuleInfo
    {
        public static readonly object _lock = new object();

        private readonly Collection<AssemblyInfo> _assemblies = new Collection<AssemblyInfo>();

        public ModuleInfo(AssemblyLoadContext context, IModuleProvider provider, string name, AssemblyInfo assemblyInfo, Type moduleInitialiser = null) : this(context, provider, assemblyInfo, moduleInitialiser)
        {
            this.Name = name;
        }

        public ModuleInfo(AssemblyLoadContext context, IModuleProvider provider, AssemblyInfo assembly, Type moduleInitialiser = null)
        {
            this.Context = context;
            this.ModuleProvider = provider;
            this.Name = assembly.Assembly.GetName().Name;
            this.AssemblyInfo = assembly;
            this.ModuleInitialiser = moduleInitialiser;
            this.AllowAssemblyResolve = true;
        }

        public AssemblyLoadContext Context { get; private set; }

        public INugetyCatalogProvider Catalog
        {
            get { return this.ModuleProvider?.Catalog; }
        }

        public IModuleProvider ModuleProvider { get; private set; }

        public Type ModuleInitialiser { get; private set; }

        public bool AllowAssemblyResolve { get; set; }

        public string Name { get; }

        public string Location
        {
            get { return AssemblyInfo.Assembly.Location; }
        }

        public AssemblyInfo AssemblyInfo { get; }

        public IEnumerable<AssemblyInfo> Assemblies
        {
            get { return this._assemblies; }
        }

        public void AddModuleInitialiser(Type type)
        {
            if (!AssemblyInfo.Assembly.ExportedTypes.Contains(type)) throw new InvalidDataException(string.Format("Type '{0}' does not exist in Assembly '{1}'", type, AssemblyInfo.Assembly));
            this.ModuleInitialiser = type;
            var assembly = type.GetTypeInfo().Assembly;
            this.AddAssembly(new AssemblyInfo(assembly));
        }

        public void AddAssembly(AssemblyInfo info)
        {
            lock (_lock)
            {
                if (!this.Assemblies.Any(a => a.Assembly.GetName().Equals(info.Assembly.GetName())))
                {
                    this._assemblies.Add(info);
                }
            }
        }

        public void RemoveAssembly(AssemblyInfo info)
        {
            lock (_lock)
            {
                if (!this.Assemblies.Any(a => a.Assembly.GetName().Equals(info.Assembly.GetName())))
                {
                    this._assemblies.Remove(info);
                }
            }
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Name) ? Name : base.ToString();
        }
    }
}