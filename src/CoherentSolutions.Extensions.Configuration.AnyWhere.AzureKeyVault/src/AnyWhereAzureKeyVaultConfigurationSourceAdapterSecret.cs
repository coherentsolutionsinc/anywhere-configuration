using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public struct AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret
    {
        private const char ESCAPE_SYMBOL = '`';

        private const char VERSION_SEPARATOR_SYMBOL = ':';

        private const char ALIAS_SEPARATOR_SYMBOL = '/';

        private const int MAX_LENGTH = 56;

        public readonly string Name;

        public readonly string Version;

        public readonly string Alias;

        private static int LastIndexOf(
            ReadOnlySpan<char> input,
            char value)
        {
            for (var i = input.Length - 1; i >= 0; --i)
            {
                var c = input[i];
                if (value != c)
                {
                    continue;
                }

                if (i > 0 && (value == VERSION_SEPARATOR_SYMBOL || value == ALIAS_SEPARATOR_SYMBOL))
                {
                    for (int j = i - 1, count = 0; j >= 0; --j, ++count)
                    {
                        if (input[j] == ESCAPE_SYMBOL)
                        {
                            continue;
                        }

                        if (count % 2 == 0)
                        {
                            return i;
                        }

                        break;
                    }
                }
                else
                {
                    return i;
                }
            }

            return -1;
        }

        private static string ToString(
            ReadOnlySpan<char> input)
        {
            var value = input.Length < MAX_LENGTH
                ? stackalloc char[input.Length]
                : new char[input.Length];

            var j = 0;
            var i = 0;
            for (; i < input.Length;)
            {
                if (input[i] == ESCAPE_SYMBOL && i < input.Length - 1)
                {
                    var c = input[i + 1];
                    if (c == ESCAPE_SYMBOL || c == ALIAS_SEPARATOR_SYMBOL || c == VERSION_SEPARATOR_SYMBOL)
                    {
                        ++i;
                    }
                }

                value[j++] = input[i++];
            }

            return value.Slice(0, j).ToString();
        }

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret(
            ReadOnlySpan<char> secretString)
        {
            if (secretString.IsEmpty || secretString.IsWhiteSpace())
            {
                throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
                    "The secret name cannot be empty or whitespace.",
                    string.Empty,
                    0);
            }

            var input = secretString;
            var index = LastIndexOf(input, ':');
            if (index >= 0)
            {
                this.Version = ToString(input.Slice(index + 1).Trim());
                if (string.IsNullOrEmpty(this.Version))
                {
                    throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
                        "Unexpected name-version separator ':'.",
                        secretString.ToString(),
                        index);
                }

                input = input.Slice(0, index);
            }
            else
            {
                this.Version = string.Empty;
            }

            index = LastIndexOf(input, '/');
            if (index >= 0)
            {
                this.Alias = ToString(input.Slice(index + 1).Trim());
                if (string.IsNullOrEmpty(this.Alias))
                {
                    throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
                        "Unexpected name-alias separator '/'.",
                        secretString.ToString(),
                        index);
                }

                input = input.Slice(0, index);
            }
            else
            {
                this.Alias = string.Empty;
            }

            this.Name = ToString(input.Trim());
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
                    "Expected secret name.",
                    secretString.ToString(),
                    0);
            }
        }
    }
}