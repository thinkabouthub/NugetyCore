using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NugetyCore;
using System.IO;

namespace NugetyCore
{

    public class ModuleInfo<T> : ModuleInfo
    {
        public ModuleInfo(INugetyCatalogProvider catalog, string name, AssemblyInfo assemblyInfo, Type moduleInitialiser = null) : base(catalog, name, assemblyInfo, moduleInitialiser)
        {
        }

        public ModuleInfo(INugetyCatalogProvider catalog, AssemblyInfo assembly, Type moduleInitialiser = null) : base(catalog, assembly, moduleInitialiser)
        {
        }
    }

    public class ModuleInfo
    {
        public INugetyCatalogProvider Catalog { get; private set; }
        public ModuleInfo(INugetyCatalogProvider catalog, string name, AssemblyInfo assemblyInfo, Type moduleInitialiser = null)
        {
            this.Catalog = catalog;
            this.Name = name;
            this.AssemblyInfo = assemblyInfo;
            this.ModuleInitialiser = moduleInitialiser;
        }

        public ModuleInfo(INugetyCatalogProvider catalog, AssemblyInfo assembly, Type moduleInitialiser = null)
        {
            this.Catalog = catalog;
            this.Name = assembly.Assembly.GetName().Name;
            this.AssemblyInfo = assembly;
            this.ModuleInitialiser = moduleInitialiser;
        }

        public void AddModuleInitialiser(Type type)
        {
            if (!this.AssemblyInfo.Assembly.ExportedTypes.Contains(type))
            {
                throw new InvalidDataException(string.Format("Type '{0}' does not exist in Assembly '{1}'", type, this.AssemblyInfo.Assembly));
            }
            this.ModuleInitialiser = type;
        }

        public Type ModuleInitialiser { get; private set; }

        public static readonly object _lock = new object();

        public string Name { get; private set; }

        public string Location
        {
            get { return this.AssemblyInfo.Assembly.Location; }
        }

        public AssemblyInfo AssemblyInfo { get; private set; }

        private readonly Collection<AssemblyInfo> related = new Collection<AssemblyInfo>();

        public IEnumerable<AssemblyInfo> Related { get { return this.related; } }

        public void AddRelated(AssemblyInfo info)
        {
            lock (_lock)
            {
                this.related.Add(info);
            }
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(this.Name) ? this.Name : base.ToString();
        }
    }
}
