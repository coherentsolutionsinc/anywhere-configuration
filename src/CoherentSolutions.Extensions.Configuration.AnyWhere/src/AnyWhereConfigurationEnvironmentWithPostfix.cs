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
            T defaultValue = default,
            bool optional = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            return this.environment.GetValue($"{name}_{this.postfix}", convertFunc, defaultValue, optional);
        }

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            var length = this.postfix.Length + 1;
            return this.environment
               .GetValues()
               .Where(
                    kv =>
                    {
                        var value = kv.Key;
                        return value.Length > length
                         && value[value.Length - length] == '_'
                         && value.EndsWith(this.postfix, StringComparison.Ordinal);
                    })
               .Select(
                    kv => new KeyValuePair<string, string>(kv.Key.Substring(0, kv.Key.Length - length), kv.Value));;
        }
    }
}