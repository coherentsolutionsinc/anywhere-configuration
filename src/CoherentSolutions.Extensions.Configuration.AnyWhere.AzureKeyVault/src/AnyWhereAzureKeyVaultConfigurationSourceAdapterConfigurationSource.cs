using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationSource : IConfigurationSource
    {
        private readonly IReadOnlyDictionary<string, string> secrets;

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationSource(
            IReadOnlyDictionary<string, string> secrets)
        {
            this.secrets = secrets ?? throw new ArgumentNullException(nameof(secrets));
        }

        public IConfigurationProvider Build(
            IConfigurationBuilder builder)
        {
            return new AnyWhereAzureKeyVaultConfigurationSourceAdapterConfigurationProvider(this.secrets);
        }
    }
}