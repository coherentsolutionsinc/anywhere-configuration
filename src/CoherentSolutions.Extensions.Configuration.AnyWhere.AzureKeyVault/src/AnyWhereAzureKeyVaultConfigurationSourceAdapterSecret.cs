using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public struct AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret
    {
        public readonly string Name;

        public readonly string Version;

        public readonly string Alias;

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret(
            in string name,
            in string alias,
            in string version)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            this.Name = name;
            this.Alias = alias ?? throw new ArgumentNullException(nameof(alias));
            this.Version = version ?? throw new ArgumentNullException(nameof(version));
        }
    }
}