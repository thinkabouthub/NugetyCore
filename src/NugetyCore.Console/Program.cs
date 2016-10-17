using System;
using System.Linq;
using Nugety;

namespace NugetyCore.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var modules = new NugetyCatalog()
                .Options.SetModuleFileNameFilter("*Module*.dll")
                .FromDirectory()
                .GetModules<IModuleInitializer>();

            var instances = modules.Load();

            instances.FirstOrDefault().ConfigureServices(null);
        }
    }
}
