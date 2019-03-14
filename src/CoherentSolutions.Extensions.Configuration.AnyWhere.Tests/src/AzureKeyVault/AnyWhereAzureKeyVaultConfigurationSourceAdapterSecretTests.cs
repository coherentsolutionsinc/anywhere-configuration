using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretTests
    {
        public class Case
        {
            public string SecretString { get; }

            public string ExpectedName { get; }

            public string ExpectedAlias { get; }

            public string ExpectedVersion { get; }

            public Case(
                string secretString,
                string expectedName,
                string expectedAlias = "",
                string expectedVersion = "")
            {
                this.SecretString = secretString;
                this.ExpectedName = expectedName;
                this.ExpectedAlias = expectedAlias;
                this.ExpectedVersion = expectedVersion;
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new Case("secret-name/secret-alias:secret-version", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("    secret-name/secret-alias:secret-version", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name    /secret-alias:secret-version", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("    secret-name    /secret-alias:secret-version", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name/    secret-alias:secret-version", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name/secret-alias    :secret-version", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name/    secret-alias    :secret-version", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name/secret-alias:    secret-version", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name/secret-alias:secret-version    ", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name/secret-alias:    secret-version    ", "secret-name", "secret-alias", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name:secret-version", "secret-name", "", "secret-version"),
            };
            yield return new object[]
            {
                new Case("    secret-name:secret-version", "secret-name", "", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name    :secret-version", "secret-name", "", "secret-version"),
            };
            yield return new object[]
            {
                new Case("    secret-name    :secret-version", "secret-name", "", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name:    secret-version", "secret-name", "", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name:secret-version    ", "secret-name", "", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name:    secret-version    ", "secret-name", "", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name", "secret-name"),
            };
            yield return new object[]
            {
                new Case("    secret-name", "secret-name"),
            };
            yield return new object[]
            {
                new Case("secret-name    ", "secret-name"),
            };
            yield return new object[]
            {
                new Case("    secret-name    ", "secret-name"),
            };
            yield return new object[]
            {
                new Case("secret-name/secret-alias", "secret-name", "secret-alias"),
            };
            yield return new object[]
            {
                new Case("    secret-name/secret-alias", "secret-name", "secret-alias"),
            };
            yield return new object[]
            {
                new Case("secret-name/secret-alias    ", "secret-name", "secret-alias"),
            };
            yield return new object[]
            {
                new Case("    secret-name/secret-alias    ", "secret-name", "secret-alias"),
            };
            yield return new object[]
            {
                new Case("secret-name`/secret-name", "secret-name/secret-name"),
            };
            yield return new object[]
            {
                new Case("secret-name`:secret-name", "secret-name:secret-name"),
            };
            yield return new object[]
            {
                new Case("secret-name``secret-name", "secret-name`secret-name"),
            };
            yield return new object[]
            {
                new Case("secret-name``/secret-alias", "secret-name`", "secret-alias"),
            };
            yield return new object[]
            {
                new Case("secret-name``:secret-version", "secret-name`", "", "secret-version"),
            };
            yield return new object[]
            {
                new Case("secret-name`secret-name", "secret-name`secret-name"),
            };
            yield return new object[]
            {
                new Case("secret-name```/secret-name", "secret-name`/secret-name"),
            };
            yield return new object[]
            {
                new Case("secret-name````/secret-alias", "secret-name``", "secret-alias"),
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void Should_read_secret_name_alias_version_When_secrets_string_contains_name_andor_alias_andor_version(
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