using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Loader;
using System.IO;
using Microsoft.Extensions.DependencyModel;

namespace NugetyCore
{
    public class DirectoryModuleProvider : IDirectoryModuleProvider
    {
        public INugetyCatalogProvider Catalog { get; private set; }
        public DirectoryModuleProvider(INugetyCatalogProvider catalog)
        {
            this.Catalog = catalog;
        }

        private DirectoryLoaderOptions options;

        public DirectoryLoaderOptions Options
        {
            get
            {
                return options ?? (options = new DirectoryLoaderOptions(this));
            }
        }

        public virtual IEnumerable<ModuleInfo<T>> GetModules<T>(params string[] name)
        {
            return this.LoadFromDirectory<T>(name);
        }

        protected virtual IEnumerable<ModuleInfo<T>> LoadFromDirectory<T>(params string[] name)
        {
            var modules = new List<ModuleInfo<T>>();
            var directories = this.GetModuleDirectories(name);
            foreach (var directory in directories)
            {
                var context = new DirectoryModuleLoadContext(this, directory);
                var module = context.LoadUsingFileName<T>();
                if (module != null)
                {
                    modules.Add(module);
                }
            }
            return modules;
        }

        public virtual IEnumerable<DirectoryInfo> GetModuleDirectories(params string[] name)
        {
            var list = new Collection<DirectoryInfo>();
            if (!Directory.Exists(this.Options.Location))
            {
                throw new DirectoryNotFoundException(this.Options.Location);
            }

            var directory = new DirectoryInfo(this.Options.Location);
            var directories = directory.GetDirectories(!string.IsNullOrEmpty(this.Catalog.Options.ModuleNameFilterPattern) ? this.Catalog.Options.ModuleNameFilterPattern : "*", SearchOption.TopDirectoryOnly);
            var notFound = name.Where(n => !directories.Any(d => d.Name == n));
            if (notFound.Any())
            {
                throw new DirectoryNotFoundException(string.Format("Module Directory not found for '{0}'", string.Join(",", notFound.ToArray())));
            }
            if (name.Length > 0)
            {
                foreach (var n in name)
                {
                    var namedDirectory = directories.FirstOrDefault(d => d.Name == n);
                    if (namedDirectory != null)
                    {
                        list.Add(namedDirectory);
                    }
                }
            }
            else
            {
                foreach (var d in directories)
                {
                    list.Add(d);
                }
            }
            return list;
        }
    }
}
