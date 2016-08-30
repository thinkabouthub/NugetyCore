using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using System.IO;

namespace NugetyCore
{
    public class NugetModuleLoadContext : AssemblyLoadContext
    {
        public INugetModuleLoader Loader { get; private set; }

        public INugetyCatalogProvider Catalog { get { return this.Loader.Catalog; } }

        public DirectoryInfo Directory { get; private set; }

        public ModuleInfo ModuleInfo { get; private set; }

        public NugetModuleLoadContext(INugetModuleLoader loader, DirectoryInfo directory)
        {
            this.Loader = loader;
            this.Directory = directory;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }

        public virtual ModuleInfo<T> LoadUsingFileName<T>()
        {
            return null;
        }
    }
}
