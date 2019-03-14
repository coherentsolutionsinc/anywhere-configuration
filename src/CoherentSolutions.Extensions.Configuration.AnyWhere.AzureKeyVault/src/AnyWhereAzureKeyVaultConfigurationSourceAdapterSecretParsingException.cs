using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public class AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException : Exception
    {
        public string SecretString { get; }

        public int Position { get; }

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
            string message,
            ReadOnlySpan<char> secretString,
            int position,
            Exception innerException = null)
            : this(message, secretString.ToString(), position, innerException)
        {
        }

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
            string message,
            string secretString,
            int position,
            Exception innerException = null)
            : base(message, innerException)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            this.SecretString = secretString;
            this.Position = position;
        }
    }
}