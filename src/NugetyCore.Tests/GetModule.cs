using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NugetyCore;
using System.IO;
using Microsoft.Extensions.Options;

namespace NugetyCore.Tests
{
    public class GetModule
    {
        [Fact]
        public void Given_ModuleNameFilterPattern_When_Valid_Then_ModuleReturned()
        {
            var modules = new NugetyCatalog()
                .Options.SetModuleNameFilterPattern("Module1")
                .FromDirectory()
                .GetModules<IModuleInitializer>();

            Assert.True(modules.Any(m => m.Name == "Module1"));
        }

        [Fact]
        public void Given_ModuleNameFilterPattern_When_Invalid_Then_NoModuleReturned()
        {
            var modules = new NugetyCatalog()
                .Options.SetModuleNameFilterPattern("Module2")
                .FromDirectory()
                .GetModules<IModuleInitializer>();
                        Assert.True(!modules.Any(m => m.Name == "Module2"));
        }

        [Fact]
        public void Given_FileNameFilterPattern_When_Valid_Then_ModuleReturned()
        {
            var modules = new NugetyCatalog()
                .Options.SetFileNameFilterPattern("*Module1")
                .FromDirectory()
                .GetModules<IModuleInitializer>();

            Assert.True(!modules.Any(m => m.Name == "Module1"));
        }

        [Fact]
        public void Given_FileNameFilterPattern_When_Invalid_Then_NoModuleReturned()
        {
            var modules = new NugetyCatalog()
                .Options.SetFileNameFilterPattern("*Module2")
                .FromDirectory()
                .GetModules<IModuleInitializer>();

            Assert.True(!modules.Any(m => m.Name == "Module2"));
        }

        [Fact]
        public void Given_ModuleLocation_When_Valid_Then_ModuleReturned()
        {
            var modules = new NugetyCatalog()
                .FromDirectory("Modules")
                .GetModules<IModuleInitializer>();

            Assert.True(modules.Any(m => m.Name == "Module1"));
            Assert.True(modules.Any(m => m.Name == "Module2 with dependency1 v0"));
            Assert.True(modules.Any(m => m.Name == "Module2 without dependency1"));
        }

        [Fact]
        public void Given_ModuleLocation_When_Invalid_Then_ThrowsDirectoryNotFoundException()
        {
            Assert.Throws<DirectoryNotFoundException>(() =>
            {
                var modules = new NugetyCatalog()
                   .FromDirectory("InvalidDirectory")
                   .GetModules<IModuleInitializer>();
            });
        }

        [Fact]
        public void Given_ModuleName_When_Valid_Then_ModuleReturned()
        {
            var modules = new NugetyCatalog()
                .FromDirectory()
                .GetModules<IModuleInitializer>("Module1");

            Assert.True(modules.Any(m => m.Name == "Module1"));
        }

        [Fact]
        public void Given_ModuleName_When_InValid_Then_ThrowsDirectoryNotFoundException()
        {
            var exception = Assert.Throws<DirectoryNotFoundException>(() =>
            {
                var modules = new NugetyCatalog()
                    .FromDirectory()
                    .GetModules<IModuleInitializer>("InvalidModule1", "InvalidModule2");
            });
            Assert.Equal("Module Directory not found for 'InvalidModule1,InvalidModule2'", exception.Message);
        }

        [Fact]
        public void Given_ModuleName_When_InValidAndValid_Then_ThrowsDirectoryNotFoundException()
        {
            var exception = Assert.Throws<DirectoryNotFoundException>(() =>
            {
                var modules = new NugetyCatalog()
                    .FromDirectory()
                    .GetModules<IModuleInitializer>("Module1", "InvalidModule");
            });
            Assert.Equal("Module Directory not found for 'InvalidModule'", exception.Message);
        }

        [Fact]
        public void Given_GetManyModules_When_Valid_Then_ModulesReturned()
        {
            var modules = new NugetyCatalog().GetMany
                (
                    c => c.FromDirectory().GetModules<IModuleInitializer>("Module1"),
                    c => c.FromDirectory().GetModules<IModuleInitializer>("Module2 with dependency1 v0")
                );

            Assert.True(modules.Any(m => m.Name == "Module1"));
            Assert.True(modules.Any(m => m.Name == "Module2 with dependency1 v0"));
        }

        [Fact]
        public void Given_Module_When_Valid_Then_PropertiesPopulated()
        {
            var modules = new NugetyCatalog()
                .FromDirectory()
                .GetModules<IModuleInitializer>("Module1");

            var module = modules.FirstOrDefault(m => m.Name == "Module1");
            Assert.True(module != null);
            Assert.NotNull(module.Catalog);
            Assert.NotNull(module.ModuleInitialiser);
            Assert.True(!string.IsNullOrEmpty(module.Location));
            Assert.NotNull(module.AssemblyInfo);
            Assert.NotNull(module.AssemblyInfo.Assembly);
            Assert.NotNull(module.AssemblyInfo.Context);
            Assert.True(!string.IsNullOrEmpty(module.AssemblyInfo.Location));
        }
    }
}
