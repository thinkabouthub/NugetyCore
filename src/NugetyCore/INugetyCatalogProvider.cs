using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nugety
{
    public interface INugetyCatalogProvider
    {
        NugetyOptions Options { get; }
        event EventHandler<ModuleIntanceEventArgs> ModuleLoaded;

        event EventHandler<CancelModuleEventArgs> ModuleLoading;

        IEnumerable<ModuleInfo<T>> GetMany<T>(params Func<INugetyCatalogProvider, IEnumerable<ModuleInfo<T>>>[] loaders);

        T Load<T>(ModuleInfo module);

        IEnumerable<T> Load<T>(IEnumerable<ModuleInfo> modules);

        IDirectoryModuleProvider FromDirectory(string location = "Modules");

        Type GetModuleInitializer<T>(Assembly assembly);

        Type GetModuleInitializer(Assembly assembly, Type initialiser);
    }
}