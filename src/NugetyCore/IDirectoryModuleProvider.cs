namespace NugetyCore
{
    public interface IDirectoryModuleProvider : IModuleProvider
    {
        DirectoryLoaderOptions Options { get; }
    }
}