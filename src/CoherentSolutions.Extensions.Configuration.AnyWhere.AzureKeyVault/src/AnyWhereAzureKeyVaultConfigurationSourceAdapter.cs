using System;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapter : IAnyWhereConfigurationAdapter
    {
        public void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder,
            IAnyWhereConfigurationEnvironmentReader environmentReader)
        {
            if (configurationBuilder is null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            if (environmentReader is null)
            {
                throw new ArgumentNullException(nameof(environmentReader));
            }

            var vaultBaseUri = environmentReader.GetString("VAULT");
            var secretsString = environmentReader.GetString("SECRETS");

            if (string.IsNullOrWhiteSpace(vaultBaseUri))
            {
                throw new ArgumentException("The 'VAULT' variable shouldn't has empty value");
            }

            if (string.IsNullOrWhiteSpace(secretsString))
            {
                throw new ArgumentException("The 'SECRETS' variable shouldn't has empty value");
            }

            var secrets = new List<AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret>();
            foreach (var secret in new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerable(secretsString))
            {
                secrets.Add(secret);
            }

            var clientId = environmentReader.GetString("CLIENT_ID", optional: true);
            var clientSecret = environmentReader.GetString("CLIENT_SECRET", optional: true);

            var client = string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret)
                ? AnyWhereAzureKeyVaultConfigurationSourceAdapterKeyVaultClientFactory.CreateClientUsingManagedIdentity()
                : AnyWhereAzureKeyVaultConfigurationSourceAdapterKeyVaultClientFactory.CreateClientUsingServicePrinciple(clientId, clientSecret);

            configurationBuilder.Add(
                new AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationSource(vaultBaseUri, client, secrets));
        }
    }
}