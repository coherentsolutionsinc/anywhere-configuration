using System;
using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationEnvironmentWithPostfix : IAnyWhereConfigurationEnvironment
    {
        private readonly IAnyWhereConfigurationEnvironment environment;

        private readonly string postfix;

        public AnyWhereConfigurationEnvironmentWithPostfix(
            IAnyWhereConfigurationEnvironment environment,
            string postfix)
        {
            if (string.IsNullOrWhiteSpace(postfix))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(postfix));
            }

            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.postfix = postfix;
        }

        public T GetValue<T>(
            string name,
            Func<string, (T value, bool converted)> convertFunc,
            T defaultValue = default(T),
            bool optional = false)
        {
            return this.environment.GetValue($"{name}_{this.postfix}", convertFunc, defaultValue, optional);
        }

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            return this.environment.GetValues().Where(kv => kv.Key.EndsWith($"_{this.postfix}"));
        }
    }
}