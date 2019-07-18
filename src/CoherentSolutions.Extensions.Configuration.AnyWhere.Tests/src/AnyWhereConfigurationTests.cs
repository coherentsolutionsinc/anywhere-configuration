using System;
using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;
using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools;

using Microsoft.Extensions.Configuration;

using Moq;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class TestAnyWhereConfiguration : AnyWhereConfiguration
    {
        private IAnyWhereConfigurationFileSearch search;

        private IAnyWhereConfigurationTypeSystem system;

        public TestAnyWhereConfiguration(
            IAnyWhereConfigurationAdapterArguments adapterArguments,
            IAnyWhereConfigurationAdapterProbingPaths adapterProbingPaths)
            : base(adapterArguments, adapterProbingPaths)
        {
        }

        public TestAnyWhereConfiguration Setup(
            IAnyWhereConfigurationFileSearch search,
            IAnyWhereConfigurationTypeSystem system)
        {
            this.search = search;
            this.system = system;

            return this;
        }

        protected override IAnyWhereConfigurationFileSearch GetSearch()
        {
            return this.search;
        }

        protected override IAnyWhereConfigurationTypeSystem GetTypeSystem()
        {
            return this.system;
        }
    }
    public class AnyWhereConfigurationTests
    {
        [Fact]
        public void Should_throw_exception_When_adapter_not_found()
        {
            var cb = new Mock<IConfigurationBuilder>();
            var args = new Mock<IAnyWhereConfigurationAdapterArguments>();
            var paths = new Mock<IAnyWhereConfigurationAdapterProbingPaths>();
            var search = new Mock<IAnyWhereConfigurationFileSearch>();
            var system = new Mock<IAnyWhereConfigurationTypeSystem>();

            var argDefinition = new AnyWhereConfigurationAdapterDefinition("type", "assembly");
            args
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new []
                    {
                        new AnyWhereConfigurationAdapterArgument(
                            argDefinition,
                            new Mock<IAnyWhereConfigurationEnvironment>().Object)
                    })
               .Verifiable();

            var path = new AnyWhereConfigurationDataPath("bin");
            paths
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new[]
                    {
                        path
                    });

            search
               .Setup(instance => instance.Find(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<string>(), It.IsAny<string[]>()))
               .Returns(Array.Empty<IAnyWhereConfigurationFileSearchResult>())
               .Verifiable();

            try
            {
                new TestAnyWhereConfiguration(args.Object, paths.Object)
                   .Setup(search.Object, system.Object)
                   .ConfigureAppConfiguration(cb.Object);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    e.Message, 
                    AnyWhereConfigurationExceptions.EmptySearchResultsMessage(
                        argDefinition.AssemblyName, 
                        new []
                        {
                            path.Value
                        }));
            }

            args.VerifyAll();
            search.VerifyAll();
        }

        [Fact]
        public void Should_throw_exception_When_adapter_files_are_found_in_multiple_directories()
        {
            var cb = new Mock<IConfigurationBuilder>();
            var args = new Mock<IAnyWhereConfigurationAdapterArguments>();
            var paths = new Mock<IAnyWhereConfigurationAdapterProbingPaths>();
            var search = new Mock<IAnyWhereConfigurationFileSearch>();
            var system = new Mock<IAnyWhereConfigurationTypeSystem>();

            args
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new []
                    {
                        new AnyWhereConfigurationAdapterArgument(
                            new AnyWhereConfigurationAdapterDefinition("type", "assembly"),
                            new Mock<IAnyWhereConfigurationEnvironment>().Object)
                    })
               .Verifiable();

            paths
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new[]
                    {
                        new AnyWhereConfigurationDataPath("bin"),
                        new AnyWhereConfigurationDataPath("vars")
                    });

            var searchResults = new[]
            {
                AnyWhereConfigurationFileSearchResultMockFactory.Create("bin", "assembly.exe"),
                AnyWhereConfigurationFileSearchResultMockFactory.Create("vars", "assembly.dll")
            };
            search
               .Setup(instance => instance.Find(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<string>(), It.IsAny<string[]>()))
               .Returns(searchResults)
               .Verifiable();

            try
            {
                new TestAnyWhereConfiguration(args.Object, paths.Object)
                   .Setup(search.Object, system.Object)
                   .ConfigureAppConfiguration(cb.Object);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    e.Message, 
                    AnyWhereConfigurationExceptions.AmbiguousSearchResultsMessage(searchResults));
            }

            args.VerifyAll();
            search.VerifyAll();
        }

        [Fact]
        public void Should_pass_environment_to_adapter_configuration_method_When_environment_is_configured()
        {
            var cb = new Mock<IConfigurationBuilder>();
            var args = new Mock<IAnyWhereConfigurationAdapterArguments>();
            var paths = new Mock<IAnyWhereConfigurationAdapterProbingPaths>();
            var search = new Mock<IAnyWhereConfigurationFileSearch>();
            var system = new Mock<IAnyWhereConfigurationTypeSystem>();

            var argEnv = new Mock<IAnyWhereConfigurationEnvironment>();
            var argDefinition = new AnyWhereConfigurationAdapterDefinition("type", "assembly");
            args
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new []
                    {
                        new AnyWhereConfigurationAdapterArgument(argDefinition, argEnv.Object)
                    })
               .Verifiable();

            paths
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new[]
                    {
                        new AnyWhereConfigurationDataPath("bin")
                    });

            var searchResult = AnyWhereConfigurationFileSearchResultMockFactory.Create("bin", "assembly.exe", null, null);
            search
               .Setup(instance => instance.Find(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<string>(), It.IsAny<string[]>()))
               .Returns(new[]
                {
                    searchResult
                })
               .Verifiable();

            var adapter = new Mock<IAnyWhereConfigurationAdapter>();
            adapter
               .Setup(instance => instance.ConfigureAppConfiguration(cb.Object, It.IsAny<IAnyWhereConfigurationEnvironmentReader>()))
               .Callback(
                    (
                        IConfigurationBuilder c,
                        IAnyWhereConfigurationEnvironmentReader e) =>
                    {
                        Assert.Same(argEnv.Object, e.Environment);
                    })
               .Verifiable();

            var type = new Mock<IAnyWhereConfigurationType>();
            type
               .Setup(instance => instance.CreateInstance())
               .Returns(adapter.Object)
               .Verifiable();

            system
               .Setup(instance => instance.Get(searchResult.Files[0], argDefinition.TypeName))
               .Returns(type.Object)
               .Verifiable();

            new TestAnyWhereConfiguration(args.Object, paths.Object)
               .Setup(search.Object, system.Object)
               .ConfigureAppConfiguration(cb.Object);

            args.VerifyAll();
            search.VerifyAll();
            system.VerifyAll();
            type.VerifyAll();
            adapter.VerifyAll();
        }

        [Fact]
        public void Should_throw_exception_When_extended_configuration_is_invalid()
        {
            var cb = new Mock<IConfigurationBuilder>();
            var args = new Mock<IAnyWhereConfigurationAdapterArguments>();
            var paths = new Mock<IAnyWhereConfigurationAdapterProbingPaths>();
            var search = new Mock<IAnyWhereConfigurationFileSearch>();
            var system = new Mock<IAnyWhereConfigurationTypeSystem>();

            var argEnv = new Mock<IAnyWhereConfigurationEnvironment>();
            var argDefinition = new AnyWhereConfigurationAdapterDefinition("type", "assembly");
            args
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new []
                    {
                        new AnyWhereConfigurationAdapterArgument(argDefinition, argEnv.Object)
                    })
               .Verifiable();

            paths
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new[]
                    {
                        new AnyWhereConfigurationDataPath("bin")
                    });

            var searchResult = AnyWhereConfigurationFileSearchResultMockFactory.Create(
                "bin", 
                ("assembly.exe", null), 
                (null, null), 
                ("assembly.anywhere", "KEY"));

            search
               .Setup(instance => instance.Find(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<string>(), It.IsAny<string[]>()))
               .Returns(new[]
                {
                    searchResult
                })
               .Verifiable();

            try
            {
                new TestAnyWhereConfiguration(args.Object, paths.Object)
                   .Setup(search.Object, system.Object)
                   .ConfigureAppConfiguration(cb.Object);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    e.Message, 
                    AnyWhereConfigurationExceptions.ErrorLoadingEnvironmentConfigurationMessage(searchResult.Files[2].Path));
            }

            args.VerifyAll();
            search.VerifyAll();
            system.VerifyAll();
        }

        [Fact]
        public void Should_pass_extended_environment_to_adapter_configuration_method_When_extended_environment_is_configured()
        {
            var cb = new Mock<IConfigurationBuilder>();
            var args = new Mock<IAnyWhereConfigurationAdapterArguments>();
            var paths = new Mock<IAnyWhereConfigurationAdapterProbingPaths>();
            var search = new Mock<IAnyWhereConfigurationFileSearch>();
            var system = new Mock<IAnyWhereConfigurationTypeSystem>();

            var argEnv = new Mock<IAnyWhereConfigurationEnvironment>();
            var argDefinition = new AnyWhereConfigurationAdapterDefinition("type", "assembly");
            args
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new []
                    {
                        new AnyWhereConfigurationAdapterArgument(argDefinition, argEnv.Object)
                    })
               .Verifiable();

            paths
               .Setup(instance => instance.Enumerate())
               .Returns(
                    new[]
                    {
                        new AnyWhereConfigurationDataPath("bin")
                    });

            var searchResult = AnyWhereConfigurationFileSearchResultMockFactory.Create(
                "bin", 
                ("assembly.exe", null), 
                (null, null), 
                ("assembly.anywhere", "KEY=VALUE"));

            search
               .Setup(instance => instance.Find(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<string>(), It.IsAny<string[]>()))
               .Returns(new[]
                {
                    searchResult
                })
               .Verifiable();

            var adapter = new Mock<IAnyWhereConfigurationAdapter>();
            adapter
               .Setup(instance => instance.ConfigureAppConfiguration(cb.Object, It.IsAny<IAnyWhereConfigurationEnvironmentReader>()))
               .Callback(
                    (
                        IConfigurationBuilder c,
                        IAnyWhereConfigurationEnvironmentReader e) =>
                    {
                        Assert.NotSame(argEnv.Object, e.Environment);
                        Assert.Equal("VALUE", e.GetString("KEY"));
                    })
               .Verifiable();

            var type = new Mock<IAnyWhereConfigurationType>();
            type
               .Setup(instance => instance.CreateInstance())
               .Returns(adapter.Object)
               .Verifiable();

            system
               .Setup(instance => instance.Get(searchResult.Files[0], argDefinition.TypeName))
               .Returns(type.Object)
               .Verifiable();

            new TestAnyWhereConfiguration(args.Object, paths.Object)
               .Setup(search.Object, system.Object)
               .ConfigureAppConfiguration(cb.Object);

            args.VerifyAll();
            search.VerifyAll();
            system.VerifyAll();
            type.VerifyAll();
            adapter.VerifyAll();
        }
    }
}