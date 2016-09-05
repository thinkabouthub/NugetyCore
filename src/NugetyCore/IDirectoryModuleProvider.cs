namespace Nugety
{
    public interface IDirectoryModuleProvider : IModuleProvider
    {
        DirectoryLoaderOptions Options { get; }
    }
}