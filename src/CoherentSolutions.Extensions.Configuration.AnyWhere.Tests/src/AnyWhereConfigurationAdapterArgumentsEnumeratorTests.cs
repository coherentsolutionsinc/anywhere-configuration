using System;
using System.Collections.Generic;
using System.IO;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Moq;

using Xunit;

using static CoherentSolutions.Extensions.Configuration.AnyWhere.AnyWhereConfigurationAdapterArgumentsEnumerator;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationAdapterArgumentsEnumeratorTests
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
                        ($"1_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "FirstAdapter")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationAdapterMetadata>
            {
                ["ZeroAdapter"] = new AnyWhereConfigurationAdapterMetadata("ZeroAdapter", "ZeroAdapterType", "ZeroAdapterAssembly"),
                ["FirstAdapter"] = new AnyWhereConfigurationAdapterMetadata("FirstAdapter", "FirstAdapterType", "FirstAdapterAssembly"),
            };

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly"),
                new AnyWhereConfigurationAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "FirstAdapterType",
                    "FirstAdapterAssembly")
            };

            var enumerator = new AnyWhereConfigurationAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
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
                        ($"1_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "FirstAdapterType")
                    },
                    new[]
                    {
                        ($"1_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "FirstAdapterAssembly")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationAdapterMetadata>();

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly"),
                new AnyWhereConfigurationAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "FirstAdapterType",
                    "FirstAdapterAssembly")
            };

            var enumerator = new AnyWhereConfigurationAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
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
                        ($"2_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "FirstAdapter")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationAdapterMetadata>
            {
                ["ZeroAdapter"] = new AnyWhereConfigurationAdapterMetadata("ZeroAdapter", "ZeroAdapterType", "ZeroAdapterAssembly")
            };

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly")
            };

            var enumerator = new AnyWhereConfigurationAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
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
                        ($"2_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "FirstAdapterType")
                    },
                    new[]
                    {
                        ($"2_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "FirstAdapterAssembly")
                    }
                });

            var adapters = new Dictionary<string, AnyWhereConfigurationAdapterMetadata>();

            var expectedArgs = new[]
            {
                new AnyWhereConfigurationAdapterArgument(
                    new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
                    "ZeroAdapterType",
                    "ZeroAdapterAssembly")
            };

            var enumerator = new AnyWhereConfigurationAdapterArgumentsEnumerator(
                adapters,
                environment.Object);

            foreach (var arg in expectedArgs)
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
                Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
            }

            Assert.False(enumerator.MoveNext());
        }
    }
}