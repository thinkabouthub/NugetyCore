using System.Collections.Generic;

namespace NugetyCore
{
    public interface IModuleProvider
    {
        INugetyCatalogProvider Catalog { get; }

        IEnumerable<ModuleInfo<T>> GetModules<T>(params string[] name);
    }
}
