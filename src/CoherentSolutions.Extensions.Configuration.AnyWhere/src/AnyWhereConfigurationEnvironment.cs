using System;
using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationEnvironment : IAnyWhereConfigurationEnvironment
    {
        private readonly Stack<IAnyWhereConfigurationEnvironmentSource> sources;

        public AnyWhereConfigurationEnvironment(
            params IAnyWhereConfigurationEnvironmentSource[] sources)
        {
            this.sources = new Stack<IAnyWhereConfigurationEnvironmentSource>(sources.Length);
            foreach (var source in sources)
            {
                this.sources.Push(source);
            }
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

            string value = null;
            foreach (var storage in this.sources)
            {
                value = storage.Get(name);
                if (value != null)
                {
                    break;
                }
            }

            if (value is null)
            {
                if (!optional)
                {
                    throw new InvalidOperationException($"The '{name}' isn't found.");
                }

                return defaultValue;
            }

            if (convertFunc is null)
            {
                if (value is T v)
                {
                    return v;
                }

                throw new InvalidCastException($"The '{name}' value '{value}' cannot be converted to {nameof(T)}.");
            }

            var (result, converted) = convertFunc(value);
            if (!converted)
            {
                throw new InvalidCastException($"The '{name}' value '{value}' cannot be converted to {nameof(T)}.");
            }

            return result;
        }

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            var set = new HashSet<string>();

            foreach (var storage in this.sources)
            {
                foreach (var kv in storage.GetValues())
                {
                    if (set.Add(kv.Key))
                    {
                        yield return kv;
                    }
                }
            }
        }
    }
}