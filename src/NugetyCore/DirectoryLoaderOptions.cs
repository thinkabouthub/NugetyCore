namespace Nugety
{
    public class DirectoryLoaderOptions
    {
        public DirectoryLoaderOptions(IDirectoryModuleProvider loader)
        {
            this.Loader = loader;
        }

        public IDirectoryModuleProvider Loader { get; }

        public string Location { get; set; }

        public IDirectoryModuleProvider SetLocation(string location)
        {
            this.Location = location;
            return Loader;
        }
    }
}