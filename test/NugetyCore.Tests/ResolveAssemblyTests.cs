using System.Linq;
using NugetyCore.Tests;
using Xunit;

namespace Nugety.Tests
{
    public class ResolveAssemblyTests
    {
        [Fact]
        public void Given_AssemblyResolve_When_DependencyNotFound_Then_DependencyFoundWithModule()
        {
            var modules = new NugetyCatalog()
                .Options.SetModuleFileNameFilter("*Module4.dll")
                .FromDirectory()
                .GetModules<IModuleInitializer>("Module4");

            var instances = modules.Load();

            instances.FirstOrDefault().ConfigureServices(null);
            Assert.True(!instances.OfType<InvalidInterface>().Any());
        }
    }
}