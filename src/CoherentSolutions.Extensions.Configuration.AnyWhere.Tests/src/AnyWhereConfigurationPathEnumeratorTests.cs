using System;
using System.Collections.Generic;
using System.IO;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationPathEnumeratorTests
    {
        public class Case : IXunitSerializable
        {
            public string PathValue { get; set; }

            public string[] ExpectedResults { get; set; }

            public Case()
            {
                this.ExpectedResults = Array.Empty<string>();
            }

            public override string ToString()
            {
                return this.PathValue;
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.PathValue), this.PathValue);
                info.AddValue(nameof(this.ExpectedResults), this.ExpectedResults);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.PathValue = info.GetValue<string>(nameof(this.PathValue));
                this.ExpectedResults = info.GetValue<string[]>(nameof(this.ExpectedResults));
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new Case()
                {
                    PathValue = "simple-path",
                    ExpectedResults = new[]
                    {
                        "simple-path"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"simple-path{Path.PathSeparator}",
                    ExpectedResults = new[]
                    {
                        "simple-path"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"{Path.PathSeparator}simple-path",
                    ExpectedResults = new[]
                    {
                        "simple-path"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"{Path.PathSeparator}simple-path{Path.PathSeparator}",
                    ExpectedResults = new[]
                    {
                        "simple-path"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  simple-path",
                    ExpectedResults = new[]
                    {
                        "simple-path"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"simple-path  ",
                    ExpectedResults = new[]
                    {
                        "simple-path"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  simple-path  ",
                    ExpectedResults = new[]
                    {
                        "simple-path"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one{Path.PathSeparator}part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"{Path.PathSeparator}part-one{Path.PathSeparator}part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one{Path.PathSeparator}part-two{Path.PathSeparator}",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"{Path.PathSeparator}part-one{Path.PathSeparator}part-two{Path.PathSeparator}",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one{Path.PathSeparator}part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one  {Path.PathSeparator}part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one  {Path.PathSeparator}part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one{Path.PathSeparator}  part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one{Path.PathSeparator}part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one{Path.PathSeparator}  part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one  {Path.PathSeparator}  part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one  {Path.PathSeparator}part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one  {Path.PathSeparator}  part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one{Path.PathSeparator}  part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one{Path.PathSeparator}part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one{Path.PathSeparator}  part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one  {Path.PathSeparator}  part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one  {Path.PathSeparator}part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one  {Path.PathSeparator}  part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one{Path.PathSeparator}  part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one  {Path.PathSeparator}  part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one  {Path.PathSeparator}  part-two",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one{Path.PathSeparator}part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one  {Path.PathSeparator}part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one  {Path.PathSeparator}part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one{Path.PathSeparator}  part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"part-one  {Path.PathSeparator}  part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    PathValue = $"  part-one  {Path.PathSeparator}  part-two  ",
                    ExpectedResults = new[]
                    {
                        "part-one",
                        "part-two"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void Should_enumerate_all_paths_From_path_string(
            Case @case)
        {
            var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
                new[]
                {
                    (AnyWhereConfigurationPathEnumerator.PATH_PARAMETER_NAME, @case.PathValue)
                });

            var enumerator = new AnyWhereConfigurationPathEnumerator(environment.Object);

            foreach (var expectedResult in @case.ExpectedResults)
            {
                Assert.True(enumerator.MoveNext());

                Assert.Equal(expectedResult, enumerator.Current.Value);
            }

            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void Should_enumerate_nothing_When_initialized_as_default_struct()
        {
            Assert.False(new AnyWhereConfigurationPathEnumerator().MoveNext());
        }

        [Fact]
        public void Should_throw_InvalidOperationException_When_accessing_Current_on_stale_enumerator()
        {
            Assert.Throws<InvalidOperationException>(
                () => { _ = new AnyWhereConfigurationPathEnumerator().Current; });
        }
    }
}