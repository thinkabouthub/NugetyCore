using NugetyCore;

namespace NugetyCore
{
    public interface INugetModuleLoader : IModuleProvider
    {
        NugetLoaderOptions Options { get; }
    }
}
