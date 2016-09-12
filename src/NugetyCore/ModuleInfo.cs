using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Nugety
{
    public class ModuleInfo<T> : ModuleInfo
    {
        public ModuleInfo(INugetyCatalogProvider catalog, string name, AssemblyInfo assemblyInfo,
            Type moduleInitialiser = null) : base(catalog, name, assemblyInfo, moduleInitialiser)
        {
        }

        public ModuleInfo(INugetyCatalogProvider catalog, AssemblyInfo assembly, Type moduleInitialiser = null)
            : base(catalog, assembly, moduleInitialiser)
        {
        }
    }

    public class ModuleInfo
    {
        public static readonly object _lock = new object();

        private readonly Collection<AssemblyInfo> related = new Collection<AssemblyInfo>();

        public ModuleInfo(INugetyCatalogProvider catalog, string name, AssemblyInfo assemblyInfo,
            Type moduleInitialiser = null)
        {
            Catalog = catalog;
            Name = name;
            AssemblyInfo = assemblyInfo;
            ModuleInitialiser = moduleInitialiser;
        }

        public ModuleInfo(INugetyCatalogProvider catalog, AssemblyInfo assembly, Type moduleInitialiser = null)
        {
            Catalog = catalog;
            Name = assembly.Assembly.GetName().Name;
            AssemblyInfo = assembly;
            ModuleInitialiser = moduleInitialiser;
        }

        public INugetyCatalogProvider Catalog { get; private set; }

        public Type ModuleInitialiser { get; private set; }

        public string Name { get; }

        public string Location
        {
            get { return AssemblyInfo.Assembly.Location; }
        }

        public AssemblyInfo AssemblyInfo { get; }

        public IEnumerable<AssemblyInfo> Related
        {
            get { return related; }
        }

        public void AddModuleInitialiser(Type type)
        {
            if (!AssemblyInfo.Assembly.ExportedTypes.Contains(type))
                throw new InvalidDataException(string.Format("Type '{0}' does not exist in Assembly '{1}'", type,
                    AssemblyInfo.Assembly));
            ModuleInitialiser = type;
        }

        public void AddRelated(AssemblyInfo info)
        {
            lock (_lock)
            {
                related.Add(info);
            }
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Name) ? Name : base.ToString();
        }
    }
}