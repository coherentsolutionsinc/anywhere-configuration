using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationDataKeyValue
    {
        public readonly string Key;

        public readonly string Value;

        public AnyWhereConfigurationDataKeyValue(
            in string key,
            in string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            }

            this.Key = key;
            this.Value = value;
        }
    }
}