using NugetyCore;
using System.Linq;
using Xunit;

namespace NugetyCore.Tests
{
    public interface InvalidInterface { }

    public class LoadModule
    {
        [Fact]
        public void Given_Initializer_When_Valid_Then_ModuleInstanceReturned()
        {
            var modules = new NugetyCatalog()
                .FromDirectory()
                .GetModules<IModuleInitializer>("Module1");

            var instances = modules.Load();
            Assert.True(instances.OfType<IModuleInitializer>().Any());
        }

        [Fact]
        public void Given_Initializer_When_Invalid_Then_NoInstancesReturned()
        {
            var modules = new NugetyCatalog()
                .FromDirectory()
                .GetModules<InvalidInterface>("Module1");

            var instances = modules.Load();
            Assert.True(!instances.OfType<InvalidInterface>().Any());
        }
    }
}
