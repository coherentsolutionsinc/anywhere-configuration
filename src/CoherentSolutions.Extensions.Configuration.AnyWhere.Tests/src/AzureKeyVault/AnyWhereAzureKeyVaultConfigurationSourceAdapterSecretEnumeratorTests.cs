using System;
using System.Collections.Generic;
using System.Text;

using CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumeratorTests
    {
        public class SuccessCase : IXunitSerializable
        {
            public class Secret : IXunitSerializable
            {
                public string ExpectedName { get; set; }

                public string ExpectedAlias { get; set; }

                public string ExpectedVersion { get; set; }

                public Secret()
                {
                    this.ExpectedAlias = string.Empty;
                    this.ExpectedVersion = string.Empty;
                }

                public override string ToString()
                {
                    var sb = new StringBuilder()
                       .Append(this.ExpectedName);

                    if (!string.IsNullOrWhiteSpace(this.ExpectedAlias))
                    {
                        sb.Append('/').Append(this.ExpectedAlias);
                    }

                    if (!string.IsNullOrWhiteSpace(this.ExpectedVersion))
                    {
                        sb.Append(':').Append(this.ExpectedVersion);
                    }

                    return sb.ToString();
                }

                public void Serialize(
                    IXunitSerializationInfo info)
                {
                    info.AddValue(nameof(this.ExpectedName), this.ExpectedName);
                    info.AddValue(nameof(this.ExpectedAlias), this.ExpectedAlias);
                    info.AddValue(nameof(this.ExpectedVersion), this.ExpectedVersion);
                }

                public void Deserialize(
                    IXunitSerializationInfo info)
                {
                    this.ExpectedName = info.GetValue<string>(nameof(this.ExpectedName));
                    this.ExpectedAlias = info.GetValue<string>(nameof(this.ExpectedAlias));
                    this.ExpectedVersion = info.GetValue<string>(nameof(this.ExpectedVersion));
                }
            }

            public string SecretsString { get; set; }

            public Secret[] Secrets { get; set; }

            public SuccessCase()
            {
                this.Secrets = Array.Empty<Secret>();
            }

            public override string ToString()
            {
                return this.SecretsString;
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.SecretsString), this.SecretsString);
                info.AddValue(nameof(this.Secrets), this.Secrets);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.SecretsString = info.GetValue<string>(nameof(this.SecretsString));
                this.Secrets = info.GetValue<Secret[]>(nameof(this.Secrets));
            }
        }

        public class ErrorCase : IXunitSerializable
        {
            public string SecretsString { get; set; }

            public int Line { get; set; }

            public int Position { get; set; }

            public override string ToString()
            {
                return this.SecretsString;
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.SecretsString), this.SecretsString);
                info.AddValue(nameof(this.Line), this.Line);
                info.AddValue(nameof(this.Position), this.Position);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.SecretsString = info.GetValue<string>(nameof(this.SecretsString));
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
                    SecretsString = "value",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value"
                        }
                    }
                },
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = ";value",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "value;",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = ";value;",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "value1;value2",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value1"
                        },
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value2"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "value1;value2;",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value1"
                        },
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value2"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = ";value1;value2",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value1"
                        },
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value2"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = ";value1;value2;",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value1"
                        },
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "value2"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = ""
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = ";"
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    ;"
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = ";    "
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    ;    "
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = ";;"
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = ";    ;"
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name/secret-alias:secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    secret-name/secret-alias:secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name    /secret-alias:secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    secret-name    /secret-alias:secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name/    secret-alias:secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name/secret-alias    :secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name/    secret-alias    :secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name/secret-alias:    secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name/secret-alias:secret-version    ",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name/secret-alias:    secret-version    ",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name:secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    secret-name:secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name    :secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    secret-name    :secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name:    secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name:secret-version    ",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name:    secret-version    ",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    secret-name",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name    ",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    secret-name    ",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name/secret-alias",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    secret-name/secret-alias",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name/secret-alias    ",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "    secret-name/secret-alias    ",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name",
                            ExpectedAlias = "secret-alias"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name`/secret-name",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name/secret-name"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name`:secret-name",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name:secret-name"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name``secret-name",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name`secret-name"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name``/secret-alias",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name`",
                            ExpectedAlias = "secret-alias"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name``:secret-version",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name`",
                            ExpectedAlias = "",
                            ExpectedVersion = "secret-version"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name`secret-name",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name`secret-name"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name```/secret-name",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name`/secret-name"
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SuccessCase()
                {
                    SecretsString = "secret-name````/secret-alias",
                    Secrets = new[]
                    {
                        new SuccessCase.Secret()
                        {
                            ExpectedName = "secret-name``",
                            ExpectedAlias = "secret-alias"
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
                    SecretsString = "secret-name/",
                    Line = 0,
                    Position = 11
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "  secret-name/",
                    Line = 0,
                    Position = 13
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "secret-name  /",
                    Line = 0,
                    Position = 13
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "  secret-name  /",
                    Line = 0,
                    Position = 15
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "/secret-alias",
                    Line = 0,
                    Position = 0
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "  /secret-alias",
                    Line = 0,
                    Position = 2
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = ":secret-version",
                    Line = 0,
                    Position = 0
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "  :secret-version",
                    Line = 0,
                    Position = 2
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "secret-name:",
                    Line = 0,
                    Position = 11
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "  secret-name:",
                    Line = 0,
                    Position = 13
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "secret-name  :",
                    Line = 0,
                    Position = 13
                },
            };
            yield return new object[]
            {
                new ErrorCase()
                {
                    SecretsString = "  secret-name  :",
                    Line = 0,
                    Position = 15
                },
            };
        }

        [Theory]
        [MemberData(nameof(GetSuccessData))]
        public void Should_enumerate_secret_When_success_secrets_string(
            SuccessCase @case)
        {
            var enumerator = new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator(@case.SecretsString);
            foreach (var secret in @case.Secrets)
            {
                Assert.True(enumerator.MoveNext());

                Assert.Equal(secret.ExpectedName, enumerator.Current.Name);
                Assert.Equal(secret.ExpectedAlias, enumerator.Current.Alias);
                Assert.Equal(secret.ExpectedVersion, enumerator.Current.Version);
            }

            Assert.False(enumerator.MoveNext());
        }

        [Theory]
        [MemberData(nameof(GetErrorData))]
        public void Should_throw_exception_When_error_secrets_string(
            ErrorCase @case)
        {
            var enumerator = new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator(@case.SecretsString);

            try
            {
                while (enumerator.MoveNext())
                {
                }
            }
            catch (AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException e)
            {
                Assert.Equal(@case.Line, e.Line);
                Assert.Equal(@case.Position, e.Position);
            }
        }

        [Fact]
        public void Should_enumerate_nothing_When_initialized_as_default_struct()
        {
            Assert.False(new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator().MoveNext());
        }

        [Fact]
        public void Should_throw_InvalidOperationException_When_accessing_Current_on_stale_enumerator()
        {
            Assert.Throws<InvalidOperationException>(
                () => { _ = new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator().Current; });
        }
    }
}