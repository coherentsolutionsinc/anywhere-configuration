using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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
                this.EnvironmentKeyValues = Array.Empty<EnvironmentKeyValue>();
                this.KnownAdapterDefinitions = Array.Empty<KnownAdapterDefinition>();
                this.ExpectedResults = Array.Empty<ExpectedResult>();
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

            public override string ToString()
            {
                var sb = new StringBuilder();

                if (this.EnvironmentKeyValues.Length > 0)
                {
                    sb.Append("env: ")
                       .AppendJoin(',', this.EnvironmentKeyValues.Select(v => $"{v.Key}={v.Value}"));
                }
                if (this.KnownAdapterDefinitions.Length > 0)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" / ");
                    }

                    sb.Append("adapters: ")
                        .AppendJoin(',', this.KnownAdapterDefinitions.Select(v => v.Name));
                }

                return sb.ToString();
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
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    EnvironmentKeyValues = new[]
                    {
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"0_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}",
                            Value = "OneType"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"0_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}",
                            Value = "OneAssembly"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"1_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}",
                            Value = "TwoType"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"1_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}",
                            Value = "TwoAssembly"
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
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    EnvironmentKeyValues = new[]
                    {
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"1_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}",
                            Value = "TwoAssembly"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"1_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}",
                            Value = "TwoType"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"0_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}",
                            Value = "OneType"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"0_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}",
                            Value = "OneAssembly"
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
                }
            };
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
                            Key = $"2_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}",
                            Value = "Three"
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
                            Name = "Three",
                            Type = "ThreeType",
                            Assembly = "ThreeAssembly"
                        }
                    },
                    ExpectedResults = new[]
                    {
                        new Case.ExpectedResult()
                        {
                            Type = "OneType",
                            Assembly = "OneAssembly"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    EnvironmentKeyValues = new[]
                    {
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"0_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}",
                            Value = "OneType"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"0_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}",
                            Value = "OneAssembly"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"2_{ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME}",
                            Value = "ThreeType"
                        },
                        new Case.EnvironmentKeyValue()
                        {
                            Key = $"2_{ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME}",
                            Value = "ThreeAssembly"
                        }
                    },
                    ExpectedResults = new[]
                    {
                        new Case.ExpectedResult()
                        {
                            Type = "OneType",
                            Assembly = "OneAssembly"
                        }
                    }
                }
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

        [Fact]
        public void Should_throw_InvalidOperationException_When_accessing_Current_on_stale_enumerator()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    _ = new AnyWhereConfigurationAdapterArgumentEnumerator().Current;
                });
        }

        [Fact]
        public void Should_throw_InvalidOperationException_When_unknown_adapter_name_used()
        {
            var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
                new[]
                {
                    ($"0_{ANYWHERE_ADAPTER_NAME_PARAMETER_NAME}", "MyAdapter")
                });

            var enumerator = new AnyWhereConfigurationAdapterArgumentEnumerator(
                environment.Object,
                new Dictionary<string, AnyWhereConfigurationAdapterDefinition>());

            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    enumerator.MoveNext();
                });
        }
    }
}