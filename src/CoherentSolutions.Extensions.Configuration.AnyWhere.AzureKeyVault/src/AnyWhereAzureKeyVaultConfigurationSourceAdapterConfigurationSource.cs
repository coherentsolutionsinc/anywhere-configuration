using System;
using System.Collections.Generic;

using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationSource : IConfigurationSource
    {
        private readonly string vaultBaseUri;

        private readonly KeyVaultClient client;

        private readonly IReadOnlyList<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret> secrets;

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationSource(
            in string vaultBaseUri,
            in KeyVaultClient client,
            in IReadOnlyList<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret> secrets)
        {
            if (string.IsNullOrWhiteSpace(vaultBaseUri))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(vaultBaseUri));
            }

            this.vaultBaseUri = vaultBaseUri;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.secrets = secrets ?? throw new ArgumentNullException(nameof(secrets));
        }

        public IConfigurationProvider Build(
            IConfigurationBuilder builder)
        {
            return new AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationProvider(
                this.vaultBaseUri,
                this.client,
                this.secrets);
        }
    }
}