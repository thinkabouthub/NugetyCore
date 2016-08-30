using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace NugetyCore
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Load<T>(this IEnumerable<ModuleInfo<T>> modules)
        {
            var instances = new List<T>();
            foreach (var module in modules)
            {
                instances.Add(module.Catalog.Load<T>(module));
            }
            return instances;
        }
    }
}
