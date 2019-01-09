using System;
using System.IO;
using System.Reflection;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

using Moq;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationAdapterAssemblyLocatorTests
    {
        [Fact]
        public void Should_find_assembly_When_multiple_paths_are_specified()
        {
            var path = Path.GetTempFileName();
            var assembly = Path.ChangeExtension(path, "dll");

            var environment = new Mock<IAnyWhereConfigurationEnvironment>();
            environment
               .Setup(
                    i => i.GetValue(
                        "PROBING_PATH",
                        It.IsAny<Func<string, (string, bool)>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
               .Returns(
                    string.Join(
                        Path.PathSeparator, 
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetAssemblyLocation()),
                        Path.GetDirectoryName(assembly)));

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
                var loader = new AnyWhereConfigurationAdapterAssemblyLocator(environment.Object);

                var location = loader.FindAssembly(Path.GetFileNameWithoutExtension(assembly));

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

            var environment = new Mock<IAnyWhereConfigurationEnvironment>();
            environment
               .Setup(
                    i => i.GetValue(
                        "PROBING_PATH",
                        It.IsAny<Func<string, (string, bool)>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
               .Returns(Path.GetDirectoryName(assembly));

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
                var loader = new AnyWhereConfigurationAdapterAssemblyLocator(environment.Object);

                var location = loader.FindAssembly(Path.GetFileNameWithoutExtension(assembly));

                Assert.Equal(assembly, location);
            }
            finally
            {
                File.Delete(assembly);
            }
        }
    }
}