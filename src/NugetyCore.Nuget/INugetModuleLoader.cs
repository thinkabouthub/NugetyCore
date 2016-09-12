namespace Nugety
{
    public interface INugetModuleLoader : IModuleProvider
    {
        NugetLoaderOptions Options { get; }
    }
}