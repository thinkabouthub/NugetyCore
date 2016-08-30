using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NugetyCore
{
    public class DirectoryLoaderOptions
    {
        public IDirectoryModuleProvider Loader { get; private set; }
        public DirectoryLoaderOptions(IDirectoryModuleProvider loader)
        {
            this.Loader = loader;
        }

        public string Location { get; set; }

        public IDirectoryModuleProvider SetLocation(string location)
        {
            this.Location = location;
            return this.Loader;
        }
    }
}
