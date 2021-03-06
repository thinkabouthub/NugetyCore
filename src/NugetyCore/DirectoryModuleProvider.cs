﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Nugety
{
    public class DirectoryModuleProvider : IDirectoryModuleProvider
    {
        private DirectoryLoaderOptions options;

        public DirectoryModuleProvider(INugetyCatalogProvider catalog)
        {
            Catalog = catalog;
        }

        public INugetyCatalogProvider Catalog { get; }

        public DirectoryLoaderOptions Options => options ?? (options = new DirectoryLoaderOptions(this)); 

        public virtual IEnumerable<ModuleInfo<T>> GetModules<T>(params string[] name)
        {
            return LoadFromDirectory<T>(name);
        }

        protected virtual IEnumerable<ModuleInfo<T>> LoadFromDirectory<T>(params string[] name)
        {
            var modules = new List<ModuleInfo<T>>();
            var directories = this.GetModuleDirectories(name);
            foreach (var directory in directories)
            {
                var module = new DirectoryModuleInfo<T>(this, directory);
                module.LoadModule<T>();
                if (module.AssemblyInfo != null && module.ModuleInitialiser != null) modules.Add(module);
            }
            return modules;
        }

        public virtual IEnumerable<DirectoryInfo> GetModuleDirectories(params string[] name)
        {
            var list = new Collection<DirectoryInfo>();
            if (!Directory.Exists(Options.Location)) throw new DirectoryNotFoundException($"Directory Catalog '{Options.Location}' does not exist");

            var directory = new DirectoryInfo(Options.Location);
            var directories = directory.GetDirectories(
                    !string.IsNullOrEmpty(Catalog.Options.ModuleNameFilterPattern)
                        ? Catalog.Options.ModuleNameFilterPattern
                        : "*", SearchOption.TopDirectoryOnly);
            var notFound = name.Where(n => !directories.Any(d => d.Name == n));

            if (notFound.Any()) throw new DirectoryNotFoundException($"Module Directory not found for '{string.Join(",", notFound.ToArray())}'");

            if (name.Length > 0)
            {
                foreach (var n in name)
                {
                    var namedDirectory = directories.FirstOrDefault(d => d.Name == n);
                    if (namedDirectory != null) list.Add(namedDirectory);
                }
            }
            else
            {
                foreach (var d in directories) list.Add(d);
            }
            return list;
        }
    }
}