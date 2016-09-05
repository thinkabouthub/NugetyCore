namespace Nugety
{
    public class DirectoryLoaderOptions
    {
        public DirectoryLoaderOptions(IDirectoryModuleProvider loader)
        {
            Loader = loader;
        }

        public IDirectoryModuleProvider Loader { get; }

        public string Location { get; set; }

        public IDirectoryModuleProvider SetLocation(string location)
        {
            Location = location;
            return Loader;
        }
    }
}