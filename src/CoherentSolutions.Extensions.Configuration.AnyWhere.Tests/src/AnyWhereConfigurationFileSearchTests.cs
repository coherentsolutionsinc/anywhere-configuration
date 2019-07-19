using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Moq;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationFileSearchTests
    {
        public class SuccessCaseFilesInDirectory : IXunitSerializable
        {
            public string File { get; set; }

            public string Directory { get; set; }

            public string[] Files { get; set; }

            public string[] Extensions { get; set; }

            public string[] ExpectedResults { get; set; }

            public SuccessCaseFilesInDirectory()
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

        public class SuccessCaseFilesInDirectories : IXunitSerializable
        {
            public class ExpectedResult : IXunitSerializable
            {
                public string Directory { get; set; }

                public string[] Files { get; set; }

                public void Serialize(
                    IXunitSerializationInfo info)
                {
                    info.AddValue(nameof(this.Directory), this.Directory);
                    info.AddValue(nameof(this.Files), this.Files);
                }

                public void Deserialize(
                    IXunitSerializationInfo info)
                {
                    this.Directory = info.GetValue<string>(nameof(this.Directory));
                    this.Files = info.GetValue<string[]>(nameof(this.Files));
                }
            }

            public string File { get; set; }

            public string[] Extensions { get; set; }

            public string[] Files { get; set; }

            public ExpectedResult[] ExpectedResults { get; set; }

            public SuccessCaseFilesInDirectories()
            {
                this.Extensions = Array.Empty<string>();
                this.Files = Array.Empty<string>();
                this.ExpectedResults = Array.Empty<ExpectedResult>();
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
                this.ExpectedResults = info.GetValue<ExpectedResult[]>(nameof(this.ExpectedResults));
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

        public static IEnumerable<object[]> GetSuccessFilesInDirectoryData()
        {
            yield return new object[]
            {
                new SuccessCaseFilesInDirectory()
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
                new SuccessCaseFilesInDirectory()
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
                new SuccessCaseFilesInDirectory()
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
                new SuccessCaseFilesInDirectory()
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
                new SuccessCaseFilesInDirectory()
                {
                    File = "assembly.assembly",
                    Directory = "bin",
                    Extensions = new[]
                    {
                        ".exe"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.assembly.exe"
                    },
                    ExpectedResults = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.assembly.exe",
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCaseFilesInDirectory()
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
                new SuccessCaseFilesInDirectory()
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

        public static IEnumerable<object[]> GetSuccessFilesInDirectoriesData()
        {
            yield return new object[]
            {
                new SuccessCaseFilesInDirectories()
                {
                    File = "assembly",
                    ExpectedResults = Array.Empty<SuccessCaseFilesInDirectories.ExpectedResult>()
                }
            };
            yield return new object[]
            {
                new SuccessCaseFilesInDirectories()
                {
                    File = "assembly",
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly"
                    },
                    ExpectedResults = new[]
                    {
                        new SuccessCaseFilesInDirectories.ExpectedResult()
                        { 
                            Directory = "bin",
                            Files = new []
                            {
                                $"bin{Path.DirectorySeparatorChar}assembly"
                            }
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCaseFilesInDirectories()
                {
                    File = "assembly",
                    Extensions = new[]
                    {
                        ".exe"
                    },
                    ExpectedResults = Array.Empty<SuccessCaseFilesInDirectories.ExpectedResult>()
                }
            };
            yield return new object[]
            {
                new SuccessCaseFilesInDirectories()
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
                        new SuccessCaseFilesInDirectories.ExpectedResult()
                        { 
                            Directory = "bin",
                            Files = new []
                            {
                                $"bin{Path.DirectorySeparatorChar}assembly.exe"
                            }
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCaseFilesInDirectories()
                {
                    File = "assembly.assembly",
                    Extensions = new[]
                    {
                        ".exe"
                    },
                    Files = new[]
                    {
                        $"bin{Path.DirectorySeparatorChar}assembly.assembly.exe"
                    },
                    ExpectedResults = new[]
                    {
                        new SuccessCaseFilesInDirectories.ExpectedResult()
                        { 
                            Directory = "bin",
                            Files = new []
                            {
                                $"bin{Path.DirectorySeparatorChar}assembly.assembly.exe"
                            }
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCaseFilesInDirectories()
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
                        new SuccessCaseFilesInDirectories.ExpectedResult()
                        { 
                            Directory = "bin",
                            Files = new []
                            {
                                $"bin{Path.DirectorySeparatorChar}assembly.exe",
                                null
                            }
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCaseFilesInDirectories()
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
                        new SuccessCaseFilesInDirectories.ExpectedResult()
                        { 
                            Directory = "bin",
                            Files = new []
                            {
                                $"bin{Path.DirectorySeparatorChar}assembly.exe",
                                $"bin{Path.DirectorySeparatorChar}assembly.dll",
                            }
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCaseFilesInDirectories()
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
                        new SuccessCaseFilesInDirectories.ExpectedResult()
                        { 
                            Directory = "bin",
                            Files = new []
                            {
                                $"bin{Path.DirectorySeparatorChar}assembly.exe",
                                null
                            }
                        },
                        new SuccessCaseFilesInDirectories.ExpectedResult()
                        { 
                            Directory = "vars",
                            Files = new []
                            {
                                null,
                                $"vars{Path.DirectorySeparatorChar}assembly.dll",
                            }
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetSuccessFilesInDirectoryData))]
        public void Should_find_all_files_in_directory(
            SuccessCaseFilesInDirectory @case)
        {
            var fs = new Mock<IAnyWhereConfigurationFileSystem>();
            foreach (var file in @case.Files)
            {
                fs.Setup(instance => instance.FileExists(file))
                   .Returns(true);
            }

            var result = new AnyWhereConfigurationFileSearch(fs.Object)
               .Find(@case.Directory, @case.File, @case.Extensions);

            Assert.Equal(@case.ExpectedResults.Length, result.Count);

            for (var i = 0; i < @case.ExpectedResults.Length; ++i)
            {
                Assert.Equal(@case.ExpectedResults[i], result[i]?.Path);
            }
        }

        [Theory]
        [MemberData(nameof(GetSuccessFilesInDirectoriesData))]
        public void Should_find_all_files_directory(
            SuccessCaseFilesInDirectories @case)
        {
            var fs = new Mock<IAnyWhereConfigurationFileSystem>();
            foreach (var file in @case.Files)
            {
                fs.Setup(instance => instance.FileExists(file))
                   .Returns(true);
            }

            var directories = @case.Files
               .Select(Path.GetDirectoryName)
               .Distinct()
               .ToArray();

            var result = new AnyWhereConfigurationFileSearch(fs.Object)
               .Find(directories, @case.File, @case.Extensions);

            Assert.Equal(@case.ExpectedResults.Length, result.Count);

            for (var i = 0; i < @case.ExpectedResults.Length; ++i)
            {
                Assert.Equal(@case.ExpectedResults[i].Directory, result[i].Directory);
                Assert.Equal(@case.ExpectedResults[i].Files.Length, result[i].Files.Count);
                for (var j = 0; j < @case.ExpectedResults[i].Files.Length; ++j)
                {
                    Assert.Equal(@case.ExpectedResults[i].Files[j], result[i].Files[j]?.Path);
                }
            }
        }
    }
}