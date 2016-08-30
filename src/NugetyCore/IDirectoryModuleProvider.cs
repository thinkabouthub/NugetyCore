using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;


namespace NugetyCore
{
    public interface IDirectoryModuleProvider : IModuleProvider
    {
        DirectoryLoaderOptions Options { get; }
    }
}
