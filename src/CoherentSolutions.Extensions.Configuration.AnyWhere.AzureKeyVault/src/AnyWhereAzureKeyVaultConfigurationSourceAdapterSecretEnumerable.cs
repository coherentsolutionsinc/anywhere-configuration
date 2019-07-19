using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public struct AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerable
    {
        private readonly string value;

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerable(
            in string value)
        {
            this.value = value;
        }

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator GetEnumerator()
        {
            return new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator(this.value);
        }
    }
}