using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretTests
    {
        public class Case : IXunitSerializable
        {
            public string SecretString { get; set; }

            public string ExpectedName { get; set; }

            public string ExpectedAlias { get; set; }

            public string ExpectedVersion { get; set; }

            public Case()
            {
                this.ExpectedAlias = string.Empty;
                this.ExpectedVersion = string.Empty;
            }

            public override string ToString()
            {
                return this.SecretString;
            }

            public void Serialize(
                IXunitSerializationInfo info)
            {
                info.AddValue(nameof(this.SecretString), this.SecretString);
                info.AddValue(nameof(this.ExpectedName), this.ExpectedName);
                info.AddValue(nameof(this.ExpectedAlias), this.ExpectedAlias);
                info.AddValue(nameof(this.ExpectedVersion), this.ExpectedVersion);
            }

            public void Deserialize(
                IXunitSerializationInfo info)
            {
                this.SecretString = info.GetValue<string>(nameof(this.SecretString));
                this.ExpectedName = info.GetValue<string>(nameof(this.ExpectedName));
                this.ExpectedAlias = info.GetValue<string>(nameof(this.ExpectedAlias));
                this.ExpectedVersion = info.GetValue<string>(nameof(this.ExpectedVersion));
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name/secret-alias:secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "    secret-name/secret-alias:secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name    /secret-alias:secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "    secret-name    /secret-alias:secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name/    secret-alias:secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name/secret-alias    :secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name/    secret-alias    :secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name/secret-alias:    secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name/secret-alias:secret-version    ",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name/secret-alias:    secret-version    ",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name:secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "    secret-name:secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name    :secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "    secret-name    :secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name:    secret-version",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name:secret-version    ",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name:    secret-version    ",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name",
                    ExpectedName = "secret-name"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "    secret-name",
                    ExpectedName = "secret-name"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name    ",
                    ExpectedName = "secret-name"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "    secret-name    ",
                    ExpectedName = "secret-name"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name/secret-alias",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "    secret-name/secret-alias",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name/secret-alias    ",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "    secret-name/secret-alias    ",
                    ExpectedName = "secret-name",
                    ExpectedAlias = "secret-alias"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name`/secret-name",
                    ExpectedName = "secret-name/secret-name"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name`:secret-name",
                    ExpectedName = "secret-name:secret-name"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name``secret-name",
                    ExpectedName = "secret-name`secret-name"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name``/secret-alias",
                    ExpectedName = "secret-name`",
                    ExpectedAlias = "secret-alias"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name``:secret-version",
                    ExpectedName = "secret-name`",
                    ExpectedAlias = "",
                    ExpectedVersion = "secret-version"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name`secret-name",
                    ExpectedName = "secret-name`secret-name"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name```/secret-name",
                    ExpectedName = "secret-name`/secret-name"
                }
            };
            yield return new object[]
            {
                new Case()
                {
                    SecretString = "secret-name````/secret-alias",
                    ExpectedName = "secret-name``",
                    ExpectedAlias = "secret-alias"
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void Should_read_secret_name_alias_version_From_secrets_string(
            Case @case)
        {
            var secret = new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret(@case.SecretString);

            Assert.Equal(@case.ExpectedName, secret.Name);
            Assert.Equal(@case.ExpectedAlias, secret.Alias);
            Assert.Equal(@case.ExpectedVersion, secret.Version);
        }

        [Fact]
        public void Should_throw_exception_When_secrets_string_has_name_alias_separator_but_has_no_alias()
        {
            Assert.Throws<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException>(
                () =>
                {
                    new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret("secret-name/");
                });
        }

        [Fact]
        public void Should_throw_exception_When_secrets_string_has_name_alias_separator_but_has_no_name()
        {
            Assert.Throws<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException>(
                () =>
                {
                    new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret("/secret-alias");
                });
        }

        [Fact]
        public void Should_throw_exception_When_secrets_string_has_name_version_separator_but_has_no_name()
        {
            Assert.Throws<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException>(
                () =>
                {
                    new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret(":secret-version");
                });
        }

        [Fact]
        public void Should_throw_exception_When_secrets_string_has_name_version_separator_but_has_no_version()
        {
            Assert.Throws<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException>(
                () =>
                {
                    new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret("secret-name:");
                });
        }

        [Fact]
        public void Should_throw_exception_When_secrets_string_is_empty()
        {
            Assert.Throws<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException>(
                () =>
                {
                    new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret("");
                });
        }

        [Fact]
        public void Should_throw_exception_When_secrets_string_is_whitespace()
        {
            Assert.Throws<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException>(
                () =>
                {
                    new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret("    ");
                });
        }
    }
}