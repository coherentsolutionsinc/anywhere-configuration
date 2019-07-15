using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Utilz;

using Moq;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationFilesTests
    {
        public class CaseFilesInDirectory : IXunitSerializable
        {
            public string File { get; set; }

            public string Directory { get; set; }

            public string[] Files { get; set; }

            public string[] Extensions { get; set; }

            public string[] ExpectedResults { get; set; }

            public CaseFilesInDirectory()
            {
                this.Files = Array.Empty<string>();
                this.Extensions = Array.Empty<string>();
                this.ExpectedResults = Array.Empty<string>();
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.File), this.File);
                info.AddValue(nameof(this.Directory), this.Directory);
                info.AddValue(nameof(this.Extensions), this.Extensions);
                info.AddValue(nameof(this.Files), this.Files);
                info.AddValue(nameof(this.ExpectedResults), this.ExpectedResults);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.File = info.GetValue<string>(nameof(this.File));
                this.Directory = info.GetValue<string>(nameof(this.Directory));
                this.Extensions = info.GetValue<string[]>(nameof(this.Extensions));
                this.Files = info.GetValue<string[]>(nameof(this.Files));
                this.ExpectedResults = info.GetValue<string[]>(nameof(this.ExpectedResults));
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                if (!string.IsNullOrEmpty(this.File))
                {
                    sb.Append("file: ")
                       .Append(this.File);
                }
                if (this.Extensions.Length > 0)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" / ");
                    }

                    sb.Append("ext: ")
                       .AppendJoin(Path.PathSeparator, this.Extensions);
                }
                if (!string.IsNullOrEmpty(this.Directory))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" / ");
                    }

                    sb.Append("dir: ")
                       .Append(this.Directory);
                }
                if (this.Files.Length > 0)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" / ");
                    }
                    sb.Append("files: ")
                       .AppendJoin(Path.PathSeparator, this.Files);
                }

                return sb.ToString();
            }
        }

        public class CaseFilesInDirectories : IXunitSerializable
        {
            public string File { get; set; }

            public string[] Extensions { get; set; }

            public string[] Files { get; set; }

            public string[] ExpectedResults { get; set; }

            public CaseFilesInDirectories()
            {
                this.Extensions = Array.Empty<string>();
                this.Files = Array.Empty<string>();
                this.ExpectedResults = Array.Empty<string>();
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.File), this.File);
                info.AddValue(nameof(this.Extensions), this.Extensions);
                info.AddValue(nameof(this.Files), this.Files);
                info.AddValue(nameof(this.ExpectedResults), this.ExpectedResults);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.File = info.GetValue<string>(nameof(this.File));
                this.Extensions = info.GetValue<string[]>(nameof(this.Extensions));
                this.Files = info.GetValue<string[]>(nameof(this.Files));
                this.ExpectedResults = info.GetValue<string[]>(nameof(this.ExpectedResults));
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                if (!string.IsNullOrEmpty(this.File))
                {
                    sb.Append("file: ")
                       .Append(this.File);
                }
                if (this.Extensions.Length > 0)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" / ");
                    }

                    sb.Append("ext: ")
                       .AppendJoin(Path.PathSeparator, this.Extensions);
                }
                if (this.Files.Length > 0)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" / ");
                    }
                    sb.Append("files: ")
                       .AppendJoin(Path.PathSeparator, this.Files);
                }

                return sb.ToString();
            }
        }

        public static IEnumerable<object[]> GetFilesInDirectoryData()
        {
            yield return new object[]
            {
                new CaseFilesInDirectory()
                {
                    File = "assembly",
                    Directory = "bin",
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.dll"
                    },
                    ExpectedResults = Array.Empty<string>()
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectory()
                {
                    File = "assembly",
                    Directory = "bin",
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly",
                    }
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectory()
                {
                    File = "assembly",
                    Directory = "bin",
                    Extensions = new[]
                    {
                        ".exe"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.dll"
                    },
                    ExpectedResults = Array.Empty<string>()
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectory()
                {
                    File = "assembly",
                    Directory = "bin",
                    Extensions = new[]
                    {
                        ".exe"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                    }
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectory()
                {
                    File = "assembly",
                    Directory = "bin",
                    Extensions = new[]
                    {
                        ".exe",
                        ".dll"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                        null
                    }
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectory()
                {
                    File = "assembly",
                    Directory = "bin",
                    Extensions = new[]
                    {
                        ".exe",
                        ".dll"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                        $"bin{Path.DirectorySeparatorChar}assembly.dll"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                        $"bin{Path.DirectorySeparatorChar}assembly.dll"
                    }
                }
            };
        }

        public static IEnumerable<object[]> GetFilesInDirectoriesData()
        {
            yield return new object[]
            {
                new CaseFilesInDirectories()
                {
                    File = "assembly",
                    ExpectedResults = Array.Empty<string>()
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectories()
                {
                    File = "assembly",
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly",
                        "bin"
                    }
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectories()
                {
                    File = "assembly",
                    Extensions = new[]
                    {
                        ".exe"
                    },
                    ExpectedResults = Array.Empty<string>()
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectories()
                {
                    File = "assembly",
                    Extensions = new[]
                    {
                        ".exe"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                        "bin"
                    }
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectories()
                {
                    File = "assembly",
                    Extensions = new[]
                    {
                        ".exe",
                        ".dll"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                        null,
                        "bin"
                    }
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectories()
                {
                    File = "assembly",
                    Extensions = new[]
                    {
                        ".exe",
                        ".dll"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                        $"bin{Path.DirectorySeparatorChar}assembly.dll"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                        $"bin{Path.DirectorySeparatorChar}assembly.dll",
                        "bin"
                    }
                }
            };
            yield return new object[]
            {
                new CaseFilesInDirectories()
                {
                    File = "assembly",
                    Extensions = new[]
                    {
                        ".exe",
                        ".dll"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                        $"vars{Path.DirectorySeparatorChar}assembly.dll"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.exe",
                        null,
                        "bin",
                        null,
                        $"vars{Path.DirectorySeparatorChar}assembly.dll",
                        "vars"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetFilesInDirectoryData))]
        public void Should_find_all_files_in_directory(
            CaseFilesInDirectory @case)
        {
            var fs = new Mock<IAnyWhereConfigurationFileSystem>();
            foreach (var file in @case.Files)
            {
                fs.Setup(instance => instance.FileExists(file))
                   .Returns(true);
            }

            var paths = new Mock<IAnyWhereConfigurationPaths>();
            paths.Setup(instance => instance.Enumerate())
               .Returns(@case.Files.Select(v => new AnyWhereConfigurationPath(Path.GetDirectoryName(v))));

            var result = new AnyWhereConfigurationFiles(fs.Object, paths.Object)
               .Find(@case.Directory, @case.File, @case.Extensions);

            Assert.Equal(@case.ExpectedResults?.ToArray(), result?.ToArray());
        }

        [Theory]
        [MemberData(nameof(GetFilesInDirectoriesData))]
        public void Should_find_all_files_directory(
            CaseFilesInDirectories @case)
        {
            var fs = new Mock<IAnyWhereConfigurationFileSystem>();
            foreach (var file in @case.Files)
            {
                fs.Setup(instance => instance.FileExists(file))
                   .Returns(true);
            }

            var paths = new Mock<IAnyWhereConfigurationPaths>();
            paths.Setup(instance => instance.Enumerate())
               .Returns(@case.Files.Select(v => new AnyWhereConfigurationPath(Path.GetDirectoryName(v))));

            var result = new AnyWhereConfigurationFiles(fs.Object, paths.Object)
               .Find(@case.File, @case.Extensions);

            Assert.Equal(@case.ExpectedResults?.ToArray(), result?.ToArray());
        }

        [Fact]
        public void Should_throw_InvalidOperationException_When_Find_is_requested_on_directory_not_in_directory_list()
        {
            var fs = new Mock<IAnyWhereConfigurationFileSystem>();
            var paths = new Mock<IAnyWhereConfigurationPaths>();
            paths.Setup(instance => instance.Enumerate())
               .Returns(new[]
                {
                    new AnyWhereConfigurationPath("bin")
                });

            var files = new AnyWhereConfigurationFiles(fs.Object, paths.Object);

            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    files.Find("var", "assembly");
                });
        }
    }
}