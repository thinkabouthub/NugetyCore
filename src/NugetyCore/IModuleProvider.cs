using System.Collections.Generic;

namespace Nugety
{
    public interface IModuleProvider
    {
        INugetyCatalogProvider Catalog { get; }

        IEnumerable<ModuleInfo<T>> GetModules<T>(params string[] name);
    }
}