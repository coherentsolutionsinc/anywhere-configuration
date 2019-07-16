using System;
using System.Collections.Generic;
using System.IO;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationDataKeyValueEnumeratorTests
    {
        public class Case : IXunitSerializable
        {
            public class KeyValue : IXunitSerializable
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

            public string DataString { get; set; }

            public KeyValue[] ExpectedResults { get; set; }

            public Case()
            {
                this.ExpectedResults = Array.Empty<KeyValue>();
            }

            public override string ToString()
            {
                return this.DataString.Replace(Environment.NewLine, "{nl}");
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.DataString), this.DataString);
                info.AddValue(nameof(this.ExpectedResults), this.ExpectedResults);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.DataString = info.GetValue<string>(nameof(this.DataString));
                this.ExpectedResults = info.GetValue<KeyValue[]>(nameof(this.ExpectedResults));
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new Case()
                {
                    DataString = "key=value",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "  key=value",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "key  =value",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "  key  =value",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "key=  value",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "key=value  ",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "key=  value  ",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "  key=  value",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "  key=value  ",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "  key=  value  ",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "key  =  value",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "key  =value  ",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "key  =  value  ",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = "key  =  value  ",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = $"{Environment.NewLine}key=value",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = $"key=value{Environment.NewLine}",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = $"name=vname{Environment.NewLine}fam=vfam",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new Case.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = $"name=vname  {Environment.NewLine}fam=vfam",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new Case.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = $"name=vname{Environment.NewLine}  fam=vfam",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new Case.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = $"name=vname  {Environment.NewLine}  fam=vfam",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new Case.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = $"name=vname{Environment.NewLine}{Environment.NewLine}fam=vfam",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new Case.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    DataString = $"name=vname{Environment.NewLine}fam=vfam{Environment.NewLine}raw=vraw",
                    ExpectedResults = new[]
                    {
                        new Case.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new Case.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        },
                        new Case.KeyValue()
                        {
                            Key = "raw",
                            Value = "vraw"
                        }
                    }
                }
            };
        }

        public static IEnumerable<object[]> GetBadlyFormattedData()
        {
            return Array.Empty<object[]>();
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void Should_enumerate_all_key_values_From_data_string(
            Case @case)
        {
            var enumerator = new AnyWhereConfigurationDataKeyValueEnumerator(@case.DataString);

            foreach (var expectedResult in @case.ExpectedResults)
            {
                Assert.True(enumerator.MoveNext());

                Assert.Equal(expectedResult.Key, enumerator.Current.Key);
                Assert.Equal(expectedResult.Value, enumerator.Current.Value);
            }

            Assert.False(enumerator.MoveNext());
        }

        //[Theory]
        //[MemberData(nameof(GetBadlyFormattedData))]
        //public void Should_throw_exception_When_data_string_is_badly_formatted(
        //    Case @case)
        //{
        //    var enumerator = new AnyWhereConfigurationDataKeyValueEnumerator(@case.DataString);

        //    Assert.Throws<InvalidOperationException>(() => { enumerator.MoveNext(); });
        //}

        [Fact]
        public void Should_enumerate_nothing_When_initialized_as_default_struct()
        {
            Assert.False(new AnyWhereConfigurationDataKeyValueEnumerator().MoveNext());
        }

        [Fact]
        public void Should_throw_InvalidOperationException_When_accessing_Current_on_stale_enumerator()
        {
            Assert.Throws<InvalidOperationException>(
                () => { _ = new AnyWhereConfigurationDataKeyValueEnumerator().Current; });
        }
    }
}