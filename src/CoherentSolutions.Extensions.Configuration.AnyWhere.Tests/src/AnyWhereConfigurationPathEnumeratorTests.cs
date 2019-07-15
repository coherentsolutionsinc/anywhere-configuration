using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Utilz;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationPathEnumeratorTests
    {
        public class Case : IXunitSerializable
        {
            public string PathString { get; set; }

            public string[] ExpectedResults { get; set; }

            public Case()
            {
                this.ExpectedResults = Array.Empty<string>();
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.PathString), this.PathString);
                info.AddValue(nameof(this.ExpectedResults), this.ExpectedResults);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.PathString = info.GetValue<string>(nameof(this.PathString));
                this.ExpectedResults = info.GetValue<string[]>(nameof(this.ExpectedResults));
            }

            public override string ToString()
            {
                return this.PathString;
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new Case()
                {
                    PathString = "simple-path",
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
                    PathString = $"simple-path{Path.PathSeparator}",
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
                    PathString = $"{Path.PathSeparator}simple-path",
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
                    PathString = $"{Path.PathSeparator}simple-path{Path.PathSeparator}",
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
                    PathString = $"  simple-path",
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
                    PathString = $"simple-path  ",
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
                    PathString = $"  simple-path  ",
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
                    PathString = $"part-one{Path.PathSeparator}part-two",
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
                    PathString = $"{Path.PathSeparator}part-one{Path.PathSeparator}part-two",
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
                    PathString = $"part-one{Path.PathSeparator}part-two{Path.PathSeparator}",
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
                    PathString = $"{Path.PathSeparator}part-one{Path.PathSeparator}part-two{Path.PathSeparator}",
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
                    PathString = $"  part-one{Path.PathSeparator}part-two",
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
                    PathString = $"part-one  {Path.PathSeparator}part-two",
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
                    PathString = $"  part-one  {Path.PathSeparator}part-two",
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
                    PathString = $"part-one{Path.PathSeparator}  part-two",
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
                    PathString = $"part-one{Path.PathSeparator}part-two  ",
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
                    PathString = $"part-one{Path.PathSeparator}  part-two  ",
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
                    PathString = $"part-one  {Path.PathSeparator}  part-two",
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
                    PathString = $"part-one  {Path.PathSeparator}part-two  ",
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
                    PathString = $"part-one  {Path.PathSeparator}  part-two  ",
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
                    PathString = $"  part-one{Path.PathSeparator}  part-two",
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
                    PathString = $"  part-one{Path.PathSeparator}part-two  ",
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
                    PathString = $"  part-one{Path.PathSeparator}  part-two  ",
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
                    PathString = $"  part-one  {Path.PathSeparator}  part-two",
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
                    PathString = $"  part-one  {Path.PathSeparator}part-two  ",
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
                    PathString = $"  part-one  {Path.PathSeparator}  part-two  ",
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
                    PathString = $"  part-one{Path.PathSeparator}  part-two",
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
                    PathString = $"part-one  {Path.PathSeparator}  part-two",
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
                    PathString = $"  part-one  {Path.PathSeparator}  part-two",
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
                    PathString = $"  part-one{Path.PathSeparator}part-two  ",
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
                    PathString = $"part-one  {Path.PathSeparator}part-two  ",
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
                    PathString = $"  part-one  {Path.PathSeparator}part-two  ",
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
                    PathString = $"  part-one{Path.PathSeparator}  part-two  ",
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
                    PathString = $"part-one  {Path.PathSeparator}  part-two  ",
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
                    PathString = $"  part-one  {Path.PathSeparator}  part-two  ",
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
            var enumerator = new AnyWhereConfigurationPathEnumerator(
                @case.PathString);

            foreach (var expectedResult in @case.ExpectedResults)
            {
                Assert.True(enumerator.MoveNext());

                Assert.Equal(expectedResult, enumerator.Current.Value);
            }

            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void Should_throw_InvalidOperationException_When_accessing_Current_on_stale_enumerator()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    _ = new AnyWhereConfigurationPathEnumerator().Current;
                });
        }
    }
}