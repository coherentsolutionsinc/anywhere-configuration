using System;
using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Utilz;

using Xunit;
using Xunit.Abstractions;

using static CoherentSolutions.Extensions.Configuration.AnyWhere.AnyWhereConfigurationAdapterArgumentEnumerator;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationAdapterArgumentEnumeratorTests
    {
        public class Case : IXunitSerializable
        {
            public class EnvironmentKeyValue : IXunitSerializable
            {
                public string Key { get; set; }

                public string Value { get; set; }

                public void Serialize(
                    IXunitSerializationInfo info)
                {
                    info.AddValue(nameof(this.Key), this.Key);
                    info.AddValue(nameof(this.Value), this.Value);
                }

                public void Deserialize(
                    IXunitSerializationInfo info)
                {
                    this.Key = info.GetValue<string>(nameof(this.Key));
                    this.Value = info.GetValue<string>(nameof(this.Value));
                }
            }

            public class KnownAdapterDefinition : IXunitSerializable
            {
                public string Name { get; set; }

                public string Type { get; set; }

                public string Assembly { get; set; }

                public void Serialize(
                    IXunitSerializationInfo info)
                {
                    info.AddValue(nameof(this.Name), this.Name);
                    info.AddValue(nameof(this.Type), this.Type);
                    info.AddValue(nameof(this.Assembly), this.Assembly);
                }

                public void Deserialize(
                    IXunitSerializationInfo info)
                {
                    this.Name = info.GetValue<string>(nameof(this.Name));
                    this.Type = info.GetValue<string>(nameof(this.Type));
                    this.Assembly = info.GetValue<string>(nameof(this.Assembly));
                }
            }

            public class ExpectedResult : IXunitSerializable
            {
                public string Type { get; set; }

                public string Assembly { get; set; }

                public void Serialize(
                    IXunitSerializationInfo info)
                {
                    info.AddValue(nameof(this.Type), this.Type);
                    info.AddValue(nameof(this.Assembly), this.Assembly);
                }

                public void Deserialize(
                    IXunitSerializationInfo info)
                {
                    this.Type = info.GetValue<string>(nameof(this.Type));
                    this.Assembly = info.GetValue<string>(nameof(this.Assembly));
                }
            }

            public EnvironmentKeyValue[] EnvironmentKeyValues { get; set; }

            public KnownAdapterDefinition[] KnownAdapterDefinitions { get; set; }

            public ExpectedResult[] ExpectedResults { get; set; }

            public Case()
            {
                this.KnownAdapterDefinitions = Array.Empty<KnownAdapterDefinition>();
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.EnvironmentKeyValues), this.EnvironmentKeyValues);
                info.AddValue(nameof(this.KnownAdapterDefinitions), this.KnownAdapterDefinitions);
                info.AddValue(nameof(this.ExpectedResults), this.ExpectedResults);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.EnvironmentKeyValues = info.GetValue<EnvironmentKeyValue[]>(nameof(this.EnvironmentKeyValues));
                this.KnownAdapterDefinitions = info.GetValue<KnownAdapterDefinition[]>(nameof(this.KnownAdapterDefinitions));
                this.ExpectedResults = info.GetValue<ExpectedResult[]>(nameof(this.ExpectedResults));
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new Case()
                {
                    EnvironmentKeyValues = new[]
                    {
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"0_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}",
                            Value = "One"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"1_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}",
                            Value = "Two"
                        }
                    },
                    KnownAdapterDefinitions = new[]
                    {
                        new Case.KnownAdapterDefinition()
                        {
                            Name = "One",
                            Type = "OneType",
                            Assembly = "OneAssembly"
                        }, 
                        new Case.KnownAdapterDefinition()
                        {
                            Name = "Two",
                            Type = "TwoType",
                            Assembly = "TwoAssembly"
                        }
                    },
                    ExpectedResults = new[]
                    {
                        new Case.ExpectedResult()
                        {
                            Type = "OneType",
                            Assembly = "OneAssembly"
                        }, 
                        new Case.ExpectedResult()
                        {
                            Type = "TwoType",
                            Assembly = "TwoAssembly"
                        }
                    }
                },
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void Should_enumerate_all_adapters_From_environment(
            Case @case)
        {
            var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
                @case.EnvironmentKeyValues.Select(i => (i.Key, i.Value)));

            var enumerator = new AnyWhereConfigurationAdapterArgumentEnumerator(
                environment.Object,
                @case.KnownAdapterDefinitions.ToDictionary(
                    i => i.Name, 
                    i => new AnyWhereConfigurationAdapterDefinition(i.Type, i.Assembly)));

            foreach (var expectedResult in @case.ExpectedResults)
            {
                Assert.True(enumerator.MoveNext());

                Assert.Equal(expectedResult.Type, enumerator.Current.Definition.TypeName);
                Assert.Equal(expectedResult.Assembly, enumerator.Current.Definition.AssemblyName);
            }

            Assert.False(enumerator.MoveNext());
        }

        //[Fact]
        //public void Should_enumerate_all_adapters_When_adapter_identified_by_type_and_assembly_names_and_isnt_preconfigured()
        //{
        //    var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
        //        new[]
        //        {
        //            new[]
        //            {
        //                ($"0_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "ZeroAdapterType")
        //            },
        //            new[]
        //            {
        //                ($"0_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "ZeroAdapterAssembly")
        //            },
        //            new[]
        //            {
        //                ($"1_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "FirstAdapterType")
        //            },
        //            new[]
        //            {
        //                ($"1_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "FirstAdapterAssembly")
        //            }
        //        });

        //    var adapters = new Dictionary<string, AnyWhereConfigurationAdapterDefinition>();

        //    var expectedArgs = new[]
        //    {
        //        new AnyWhereConfigurationAdapterArgument(
        //            new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
        //            "ZeroAdapterType",
        //            "ZeroAdapterAssembly"),
        //        new AnyWhereConfigurationAdapterArgument(
        //            new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
        //            "FirstAdapterType",
        //            "FirstAdapterAssembly")
        //    };

        //    var enumerator = new AnyWhereConfigurationAdapterArgumentEnumerator(
        //        environment.Object,
        //        adapters);

        //    foreach (var arg in expectedArgs)
        //    {
        //        Assert.True(enumerator.MoveNext());
        //        Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
        //        Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
        //    }

        //    Assert.False(enumerator.MoveNext());
        //}

        //[Fact]
        //public void Should_enumerate_only_continues_adapters_When_adapter_identified_by_name_and_is_preconfigured()
        //{
        //    var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
        //        new[]
        //        {
        //            new[]
        //            {
        //                ($"0_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "ZeroAdapter")
        //            },
        //            new[]
        //            {
        //                ($"2_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "FirstAdapter")
        //            }
        //        });

        //    var adapters = new Dictionary<string, AnyWhereConfigurationAdapterDefinition>
        //    {
        //        ["ZeroAdapter"] = new AnyWhereConfigurationAdapterDefinition("ZeroAdapter", "ZeroAdapterType", "ZeroAdapterAssembly")
        //    };

        //    var expectedArgs = new[]
        //    {
        //        new AnyWhereConfigurationAdapterArgument(
        //            new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
        //            "ZeroAdapterType",
        //            "ZeroAdapterAssembly")
        //    };

        //    var enumerator = new AnyWhereConfigurationAdapterArgumentEnumerator(
        //        environment.Object,
        //        adapters);

        //    foreach (var arg in expectedArgs)
        //    {
        //        Assert.True(enumerator.MoveNext());
        //        Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
        //        Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
        //    }

        //    Assert.False(enumerator.MoveNext());
        //}

        //[Fact]
        //public void Should_enumerate_only_continues_adapters_When_adapter_identified_by_type_and_assembly_names_and_isnt_preconfigured()
        //{
        //    var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
        //        new[]
        //        {
        //            new[]
        //            {
        //                ($"0_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "ZeroAdapterType")
        //            },
        //            new[]
        //            {
        //                ($"0_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "ZeroAdapterAssembly")
        //            },
        //            new[]
        //            {
        //                ($"2_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}", "FirstAdapterType")
        //            },
        //            new[]
        //            {
        //                ($"2_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}", "FirstAdapterAssembly")
        //            }
        //        });

        //    var adapters = new Dictionary<string, AnyWhereConfigurationAdapterDefinition>();

        //    var expectedArgs = new[]
        //    {
        //        new AnyWhereConfigurationAdapterArgument(
        //            new Mock<IAnyWhereConfigurationEnvironmentReader>().Object,
        //            "ZeroAdapterType",
        //            "ZeroAdapterAssembly")
        //    };

        //    var enumerator = new AnyWhereConfigurationAdapterArgumentEnumerator(
        //        environment.Object,
        //        adapters);

        //    foreach (var arg in expectedArgs)
        //    {
        //        Assert.True(enumerator.MoveNext());
        //        Assert.Equal(arg.AdapterTypeName, enumerator.Current.AdapterTypeName);
        //        Assert.Equal(arg.AdapterAssemblyName, enumerator.Current.AdapterAssemblyName);
        //    }

        //    Assert.False(enumerator.MoveNext());
        //}
    }
}