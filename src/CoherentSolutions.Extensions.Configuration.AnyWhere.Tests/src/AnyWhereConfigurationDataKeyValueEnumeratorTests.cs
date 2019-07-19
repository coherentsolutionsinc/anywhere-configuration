using System;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationDataKeyValueEnumeratorTests
    {
        public class SuccessCase : IXunitSerializable
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

            public SuccessCase()
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

        public class ErrorCase : IXunitSerializable
        {
            public string DataString { get; set; }

            public int Line { get; set; }

            public int Position { get; set; }

            public override string ToString()
            {
                return this.DataString.Replace(Environment.NewLine, "{nl}");
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.DataString), this.DataString);
                info.AddValue(nameof(this.Line), this.Line);
                info.AddValue(nameof(this.Position), this.Position);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.DataString = info.GetValue<string>(nameof(this.DataString));
                this.Line = info.GetValue<int>(nameof(this.Line));
                this.Position = info.GetValue<int>(nameof(this.Position));
            }
        }

        public static IEnumerable<object[]> GetSuccessData()
        {
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "key=value",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "  key=value",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "key  =value",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "  key  =value",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "key=  value",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "key=value  ",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "key=  value  ",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "  key=  value",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "  key=value  ",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "  key=  value  ",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "key  =  value",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "key  =value  ",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "key  =  value  ",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = "key  =  value  ",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = $"{Environment.NewLine}key=value",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = $"key=value{Environment.NewLine}",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "key",
                            Value = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = $"name=vname{Environment.NewLine}fam=vfam",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new SuccessCase.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = $"name=vname  {Environment.NewLine}fam=vfam",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new SuccessCase.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = $"name=vname{Environment.NewLine}  fam=vfam",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new SuccessCase.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = $"name=vname  {Environment.NewLine}  fam=vfam",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new SuccessCase.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = $"name=vname{Environment.NewLine}{Environment.NewLine}fam=vfam",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new SuccessCase.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    DataString = $"name=vname{Environment.NewLine}fam=vfam{Environment.NewLine}raw=vraw",
                    ExpectedResults = new[]
                    {
                        new SuccessCase.KeyValue()
                        {
                            Key = "name",
                            Value = "vname"
                        },
                        new SuccessCase.KeyValue()
                        {
                            Key = "fam",
                            Value = "vfam"
                        },
                        new SuccessCase.KeyValue()
                        {
                            Key = "raw",
                            Value = "vraw"
                        }
                    }
                }
            };
        }

        public static IEnumerable<object[]> GetErrorData()
        {
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = "keyvalue",
                    Line = 0,
                    Position = 0
                }
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = "key=",
                    Line = 0,
                    Position = 4
                }
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = "key=  ",
                    Line = 0,
                    Position = 4
                }
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = "=value",
                    Line = 0,
                    Position = 0
                }
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = "  =value",
                    Line = 0,
                    Position = 2
                }
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = $"key=value{Environment.NewLine}keyvalue",
                    Line = 1,
                    Position = 0
                }
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = $"key=value{Environment.NewLine}key=",
                    Line = 1,
                    Position = 4
                }
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = $"key=value{Environment.NewLine}key=  ",
                    Line = 1,
                    Position = 4
                }
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = $"key=value{Environment.NewLine}=value",
                    Line = 1,
                    Position = 0
                }
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    DataString = $"key=value{Environment.NewLine}  =value",
                    Line = 1,
                    Position = 2
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetSuccessData))]
        public void Should_enumerate_all_key_values_When_success_data_string(
            SuccessCase @case)
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

        [Theory]
        [MemberData(nameof(GetErrorData))]
        public void Should_throw_exception_When_error_data_string(
            ErrorCase @case)
        {
            var enumerator = new AnyWhereConfigurationDataKeyValueEnumerator(@case.DataString);

            try
            {
                while (enumerator.MoveNext())
                {
                }
            }
            catch (AnyWhereConfigurationParseException e)
            {
                Assert.Equal(@case.Line, e.Line);
                Assert.Equal(@case.Position, e.Position);
            }
        }

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