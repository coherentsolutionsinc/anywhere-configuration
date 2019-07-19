using System;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault;

using Xunit;
using Xunit.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumeratorTests
    {
        public class Case : IXunitSerializable
        {
            public string SecretsString { get; set; }

            public string[] Secrets { get; set; }

            public Case()
            {
                this.Secrets = Array.Empty<string>();
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
                this.Secrets = info.GetValue<string[]>(nameof(this.Secrets));
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new Case() { SecretsString = "value", Secrets = new [] { "value" } },
            };
            yield return new object[]
            {
                new Case() { SecretsString = ";value", Secrets = new [] { "value" } }
            };
            yield return new object[]
            {
                new Case() { SecretsString = "value;", Secrets = new [] { "value" } }
            };
            yield return new object[]
            {
                new Case() { SecretsString = ";value;", Secrets = new [] { "value" } }
            };
            yield return new object[]
            {
                new Case() { SecretsString = "value;value", Secrets = new [] { "value", "value" } }
            };
            yield return new object[]
            {
                new Case() { SecretsString = "value;value;", Secrets = new [] { "value", "value" } }
            };
            yield return new object[]
            {
                new Case() { SecretsString = ";value;value", Secrets = new [] { "value", "value" } }
            };
            yield return new object[]
            {
                new Case() { SecretsString = ";value;value;", Secrets = new [] { "value", "value" } }
            };
            yield return new object[]
            {
                new Case() { SecretsString = "" }
            };
            yield return new object[]
            {
                new Case() { SecretsString = ";" }
            };
            yield return new object[]
            {
                new Case() { SecretsString = "    ;" }
            };
            yield return new object[]
            {
                new Case() { SecretsString = ";    " }
            };
            yield return new object[]
            {
                new Case() { SecretsString = "    ;    " }
            };
            yield return new object[]
            {
                new Case() { SecretsString = ";;" }
            };
            yield return new object[]
            {
                new Case() { SecretsString = ";    ;" }
            };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void Should_enumerate_secret_When_there_are_secrets(
            Case @case)
        {
            var enumerator = new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator(@case.SecretsString);
            foreach (var secret in @case.Secrets)
            {
                Assert.True(enumerator.MoveNext());

                Assert.Equal(secret, enumerator.Current.Name);
            }
            Assert.False(enumerator.MoveNext());
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