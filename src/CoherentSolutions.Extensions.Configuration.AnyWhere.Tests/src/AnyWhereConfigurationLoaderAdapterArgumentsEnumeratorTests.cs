using System;
using System.Collections.Generic;
using System.IO;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Moq;

using Xunit;

using static CoherentSolutions.Extensions.Configuration.AnyWhere.AnyWhereConfigurationLoaderAdapterArgumentsEnumerator;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationLoaderAdapterArgumentsEnumeratorTests
    {
        private static Mock<IAnyWhereConfigurationEnvironment> CreateEnvironmentMock(
            IReadOnlyList<(string key, string value)[]> values)
        {
            var environment = new Mock<IAnyWhereConfigurationEnvironment>();
            for (var index = 0; index < values.Count; index++)
            {
                var set = values[index];
                foreach (var kv in set)
                {
                    environment
                       .Setup(i => i.GetValue(kv.key, It.IsAny<Func<string, (string value, bool converted)>>(), It.IsAny<string>(), It.IsAny<bool>()))
                       .Returns(kv.value)
                       .Verifiable();
                }
            }

            return environment;
        }

        [Fact]
        public void Should_default_SearchPaths_to_current_directory_When_adapter_identified_by_name_and_no_SearchPaths_is_set()
        {
            var environment = CreateEnvironmentMock(
                new[]
                {
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "ZeroAdapter")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationLoaderAdapterMetadata>
            {
                ["ZeroAdapter"] = new AnyWhereConfigurationLoaderAdapterMetadata("ZeroAdapter", "ZeroAdapterType", "ZeroAdapterAssembly")
            };

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationLoaderAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly",
                    Directory.GetCurrentDirectory())
            };

            var enumerator = new AnyWhereConfigurationLoaderAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
                Assert.Equal(arg.AdapterSearchPaths, enumerator.Current.AdapterSearchPaths);
            }

            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void Should_default_SearchPaths_to_current_directory_When_adapter_identified_by_type_and_assembly_names_and_no_SearchPaths_is_set()
        {
            var environment = CreateEnvironmentMock(
                new[]
                {
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "ZeroAdapterType")
                    },
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "ZeroAdapterAssembly")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationLoaderAdapterMetadata>();

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationLoaderAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly",
                    Directory.GetCurrentDirectory())
            };

            var enumerator = new AnyWhereConfigurationLoaderAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
                Assert.Equal(arg.AdapterSearchPaths, enumerator.Current.AdapterSearchPaths);
            }

            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void Should_enumerate_all_adapters_When_adapter_identified_by_name_and_is_preconfigured()
        {
            var environment = CreateEnvironmentMock(
                new[]
                {
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "ZeroAdapter")
                    },
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME}", "ZeroPath")
                    },
                    new[]
                    {
                        ($"1_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "FirstAdapter")
                    },
                    new[]
                    {
                        ($"1_{ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME}", "FirstPath")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationLoaderAdapterMetadata>
            {
                ["ZeroAdapter"] = new AnyWhereConfigurationLoaderAdapterMetadata("ZeroAdapter", "ZeroAdapterType", "ZeroAdapterAssembly"),
                ["FirstAdapter"] = new AnyWhereConfigurationLoaderAdapterMetadata("FirstAdapter", "FirstAdapterType", "FirstAdapterAssembly"),
            };

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationLoaderAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly",
                    "ZeroPath"),
                new AnyWhereConfigurationLoaderAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "FirstAdapterType",
                    "FirstAdapterAssembly",
                    "FirstPath"),
            };

            var enumerator = new AnyWhereConfigurationLoaderAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
                Assert.Equal(arg.AdapterSearchPaths, enumerator.Current.AdapterSearchPaths);
            }

            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void Should_enumerate_all_adapters_When_adapter_identified_by_type_and_assembly_names_and_isnt_preconfigured()
        {
            var environment = CreateEnvironmentMock(
                new[]
                {
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "ZeroAdapterType")
                    },
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "ZeroAdapterAssembly")
                    },
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME}", "ZeroPath")
                    },
                    new[]
                    {
                        ($"1_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "FirstAdapterType")
                    },
                    new[]
                    {
                        ($"1_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "FirstAdapterAssembly")
                    },
                    new[]
                    {
                        ($"1_{ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME}", "FirstPath")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationLoaderAdapterMetadata>();

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationLoaderAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly",
                    "ZeroPath"),
                new AnyWhereConfigurationLoaderAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "FirstAdapterType",
                    "FirstAdapterAssembly",
                    "FirstPath"),
            };

            var enumerator = new AnyWhereConfigurationLoaderAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
                Assert.Equal(arg.AdapterSearchPaths, enumerator.Current.AdapterSearchPaths);
            }

            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void Should_enumerate_only_continues_adapters_When_adapter_identified_by_name_and_is_preconfigured()
        {
            var environment = CreateEnvironmentMock(
                new[]
                {
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "ZeroAdapter")
                    },
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME}", "ZeroPath")
                    },
                    new[]
                    {
                        ($"2_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "FirstAdapter")
                    },
                    new[]
                    {
                        ($"2_{ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME}", "FirstPath")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationLoaderAdapterMetadata>
            {
                ["ZeroAdapter"] = new AnyWhereConfigurationLoaderAdapterMetadata("ZeroAdapter", "ZeroAdapterType", "ZeroAdapterAssembly")
            };

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationLoaderAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly",
                    "ZeroPath")
            };

            var enumerator = new AnyWhereConfigurationLoaderAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
                Assert.Equal(arg.AdapterSearchPaths, enumerator.Current.AdapterSearchPaths);
            }

            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void Should_enumerate_only_continues_adapters_When_adapter_identified_by_type_and_assembly_names_and_isnt_preconfigured()
        {
            var environment = CreateEnvironmentMock(
                new[]
                {
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "ZeroAdapterType")
                    },
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "ZeroAdapterAssembly")
                    },
                    new[]
                    {
                        ($"0_{ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME}", "ZeroPath")
                    },
                    new[]
                    {
                        ($"2_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "FirstAdapterType")
                    },
                    new[]
                    {
                        ($"2_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "FirstAdapterAssembly")
                    },
                    new[]
                    {
                        ($"2_{ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME}", "FirstPath")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationLoaderAdapterMetadata>();

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationLoaderAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly",
                    "ZeroPath")
            };

            var enumerator = new AnyWhereConfigurationLoaderAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
                Assert.Equal(arg.AdapterSearchPaths, enumerator.Current.AdapterSearchPaths);
            }

            Assert.False(enumerator.MoveNext());
        }
    }
}