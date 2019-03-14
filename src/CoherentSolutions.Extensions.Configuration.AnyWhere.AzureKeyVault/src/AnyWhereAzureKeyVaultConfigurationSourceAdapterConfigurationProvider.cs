using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationProvider : ConfigurationProvider
    {
        public AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationProvider(
            IReadOnlyDictionary<string, string> secrets)
        {
            if (secrets == null)
            {
                throw new ArgumentNullException(nameof(secrets));
            }

            foreach (var kv in secrets)
            {
                this.Data[kv.Key] = kv.Value;
            }
        }
    }
}