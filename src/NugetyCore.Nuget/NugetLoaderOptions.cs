namespace Nugety
{
    public class NugetLoaderOptions
    {
        public NugetLoaderOptions(INugetModuleLoader loader)
        {
            Loader = loader;
        }

        public INugetModuleLoader Loader { get; }

        public string Location { get; set; }

        public INugetModuleLoader SetLocation(string location)
        {
            Location = location;
            return Loader;
        }
    }
}