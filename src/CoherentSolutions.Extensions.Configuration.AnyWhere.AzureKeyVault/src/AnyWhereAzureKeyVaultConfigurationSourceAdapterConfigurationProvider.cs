using System;
using System.Collections.Generic;

using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationProvider : ConfigurationProvider
    {
        private readonly string vaultBaseUri;

        private readonly KeyVaultClient client;

        private readonly IReadOnlyList<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret> secrets;

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationProvider(
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

        public override void Load()
        {
            base.Load();

            var result = new Dictionary<string, string>(this.secrets.Count);
            foreach (var secret in this.secrets)
            {
                var name = secret.Name;
                var alias = secret.Alias;
                var version = secret.Version;

                if (string.IsNullOrEmpty(alias))
                {
                    alias = name;
                }

                result[alias] = this.client.GetSecretAsync(this.vaultBaseUri, name, version)
                   .GetAwaiter()
                   .GetResult()
                   .Value;
            }

            this.Data = result;
        }
    }
}