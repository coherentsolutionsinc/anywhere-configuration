using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterException : Exception
    {
        public AnyWhereAzureKeyVaultConfigurationSourceAdapterException(
            string message,
            Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}