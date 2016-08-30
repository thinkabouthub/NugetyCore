using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NugetyCore
{
    public class NugetLoaderOptions
    {
        public INugetModuleLoader Loader { get; private set; }
        public NugetLoaderOptions(INugetModuleLoader loader)
        {
            this.Loader = loader;
        }

        public string Location { get; set; }

        public INugetModuleLoader SetLocation(string location)
        {
            this.Location = location;
            return this.Loader;
        }
    }
}
