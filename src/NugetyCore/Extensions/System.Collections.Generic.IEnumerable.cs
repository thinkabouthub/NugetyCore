using System.Collections.Generic;

namespace Nugety
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Load<T>(this IEnumerable<ModuleInfo<T>> modules)
        {
            var instances = new List<T>();
            foreach (var module in modules)
            {
                var i = module.Catalog.Load<T>(module);
                if (i != null) instances.Add(i);
            }
            return instances;
        }
    }
}