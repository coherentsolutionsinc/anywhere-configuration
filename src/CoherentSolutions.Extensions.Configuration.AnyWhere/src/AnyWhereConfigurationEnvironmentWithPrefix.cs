using System;
using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationEnvironmentWithPrefix : IAnyWhereConfigurationEnvironment
    {
        private readonly IAnyWhereConfigurationEnvironment environment;

        private readonly string prefix;

        public AnyWhereConfigurationEnvironmentWithPrefix(
            IAnyWhereConfigurationEnvironment environment,
            string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(prefix));
            }

            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.prefix = prefix;
        }

        public T GetValue<T>(
            string name,
            Func<string, (T value, bool converted)> convertFunc,
            T defaultValue = default(T),
            bool optional = false)
        {
            return this.environment.GetValue($"{this.prefix}_{name}", convertFunc, defaultValue, optional);
        }

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            return this.environment.GetValues().Where(kv => kv.Key.StartsWith($"{this.prefix}_"));
        }
    }
}