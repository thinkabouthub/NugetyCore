using System.Linq;
using Xunit;
using System.IO;
using System.Reflection;
using NugetyCore.Tests.Common;

namespace NugetyCore.Tests
{
    public class GetModuleWithDependencies
    {
        [Fact]
        public void Given_Dependency_When_Exists_Then_ModuleReturned()
        {
            var modules = new NugetyCatalog()
                .FromDirectory()
                .GetModules<IModuleInitializer>("Module2 with dependency1 v0");

            var instances = modules.Load();

            Assert.True(modules.Any(m => m.Name == "Module2 with dependency1 v0"));
            Assert.True(instances.OfType<IModuleInitializer>().Any());
        }

        [Fact]
        public void Given_Dependency_When_DoesNotExist_Then_ThrowsException()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                var modules = new NugetyCatalog()
                    .FromDirectory()
                    .GetModules<IModuleInitializer>("Module2 without dependency1");

                var instance = modules.Load().OfType<IDependency1Version>().FirstOrDefault();
                instance.GetDependency1Type();
            });
        }

        [Fact]
        public void Given_TwoModules_When_HasSameDependencyDifferentVersion_Then_BothVersionsLoad()
        {
            var modules = new NugetyCatalog()
                .FromDirectory()
                .GetModules<IModuleInitializer>("Module2 with dependency1 v0", "Module3 with dependency1 v1");

            Assert.True(modules.Count() == 2);
            var instances = modules.Load().OfType<IDependency1Version>();
            Assert.True(instances.Count() == 2);

            var names = instances.Select(i =>  i.GetDependency1Type().GetTypeInfo().AssemblyQualifiedName);
            Assert.True(names.Any(n => n.Contains("1.0.0.0")), "Module2 did not load Dependency1 version '1.0.0.0'");
            Assert.True(names.Any(n => n.Contains("1.0.1.0")), "Module3 did not load Dependency1 version '1.0.1.0'");
        }
    }
}
