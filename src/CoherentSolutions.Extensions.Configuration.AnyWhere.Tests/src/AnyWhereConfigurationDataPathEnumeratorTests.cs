using System;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationDataPathEnumeratorTests
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

        public static IEnumerable<object[]> GetSuccessData()
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
        [MemberData(nameof(GetSuccessData))]
        public void Should_enumerate_all_paths_From_path_string(
            Case @case)
        {
            var enumerator = new AnyWhereConfigurationDataPathEnumerator(@case.PathValue);

            foreach (var expectedResult in @case.ExpectedResults)
            {
                Assert.True(enumerator.MoveNext());

                Assert.Equal(expectedResult, enumerator.Current.Path);
            }

            Assert.False(enumerator.MoveNext());
        }
    }
}