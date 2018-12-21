using System;
using System.IO;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationLoaderAssemblyLocatorTests
    {
        [Fact]
        public void Should_find_assembly_When_multiple_paths_are_specified()
        {
            var path = Path.GetTempFileName();
            var assembly = Path.ChangeExtension(path, "dll");

            try
            {
                File.Move(path, assembly);
            }
            catch
            {
                File.Delete(path);
                throw;
            }

            try
            {
                var locator = new AnyWhereConfigurationLoaderAssemblyLocator();

                var location = locator.FindAssembly(
                    new[]
                    {
                        Environment.CurrentDirectory,
                        Path.GetDirectoryName(assembly)
                    },
                    Path.GetFileName(assembly));

                Assert.Equal(assembly, location);
            }
            finally
            {
                File.Delete(assembly);
            }
        }

        [Fact]
        public void Should_find_assembly_When_single_path_is_specified()
        {
            var path = Path.GetTempFileName();
            var assembly = Path.ChangeExtension(path, "dll");

            try
            {
                File.Move(path, assembly);
            }
            catch
            {
                File.Delete(path);
                throw;
            }

            try
            {
                var locator = new AnyWhereConfigurationLoaderAssemblyLocator();

                var location = locator.FindAssembly(
                    new[]
                    {
                        Path.GetDirectoryName(assembly)
                    },
                    Path.GetFileName(assembly));

                Assert.Equal(assembly, location);
            }
            finally
            {
                File.Delete(assembly);
            }
        }
    }
}