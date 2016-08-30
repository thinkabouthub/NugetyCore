
namespace NugetyCore
{
    public static class INugetyExtensions
    {
        /// TODO: location may need to be determined based on target platform
        /// http://lastexitcode.com/projects/NuGet/FileLocations/
        public static INugetModuleLoader FromNuget(this INugetyCatalogProvider catalog, string location = @"%UserProfile%\.nuget\packages")
        {
            return new NugetModuleLoader(catalog).Options.SetLocation(location);
        }
    }
}