using CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumeratorTests
    {
        [Fact]
        public void Should_enumerate_secret_When_secrets_string_isnt_empty_or_whitespace()
        {
            foreach (var value in new[]
            {
                "value",
                ";value",
                "value;",
                ";value;",
                "value;value",
                "value;value;",
                ";value;value",
                ";value;value;",
            })
            {
                foreach (var item in new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerable(value))
                {
                    Assert.Equal("value", item.Name);
                }
            }
        }

        [Fact]
        public void Should_not_enumerate_secret_When_secrets_string_is_empty_or_whitespace()
        {
            foreach (var value in new[]
            {
                "",
                ";",
                "    ;",
                ";    ",
                "    ;    ",
                ";;",
                ";    ;"
            })
            {
                foreach (var item in new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerable(value))
                {
                    Assert.True(false, "No items should be enumerated");
                }
            }
        }
    }
}