using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nugety;

namespace NugetyCore.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var modules = new NugetyCatalog()
                .Options.SetModuleFileNameFilter("*Module*")
                .FromDirectory()
                .GetModules<IModuleInitializer>();

            var instances = modules.Load();

            instances.FirstOrDefault().ConfigureServices(null);
        }
    }
}
