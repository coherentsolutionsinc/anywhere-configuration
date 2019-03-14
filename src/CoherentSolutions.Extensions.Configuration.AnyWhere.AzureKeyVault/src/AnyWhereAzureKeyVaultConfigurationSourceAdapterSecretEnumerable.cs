using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public ref struct AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerable
    {
        private readonly ReadOnlySpan<char> value;

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerable(
            in ReadOnlySpan<char> value)
        {
            this.value = value;
        }

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator GetEnumerator()
        {
            return new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator(this.value);
        }
    }
}