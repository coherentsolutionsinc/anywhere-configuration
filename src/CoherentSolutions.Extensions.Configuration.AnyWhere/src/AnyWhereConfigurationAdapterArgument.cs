using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationAdapterArgument
    {
        public AnyWhereConfigurationAdapterDefinition Definition { get; }

        public string Key { get; }

        public AnyWhereConfigurationAdapterArgument(
            AnyWhereConfigurationAdapterDefinition definition,
            string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            this.Definition = definition;
            this.Key = key;
        }
    }
}