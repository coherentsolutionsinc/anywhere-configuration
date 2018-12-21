using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationEnvironment : IAnyWhereConfigurationEnvironment
    {
        private interface IValueStorage
        {
            string Get(
                string name);

            IEnumerable<KeyValuePair<string, string>> GetValues();
        }

        private sealed class ProcessEnvironmentValueStorage : IValueStorage
        {
            public string Get(
                string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
                }

                return Environment.GetEnvironmentVariable(name);
            }

            public IEnumerable<KeyValuePair<string, string>> GetValues()
            {
                foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
                {
                    yield return new KeyValuePair<string, string>(entry.Key as string, entry.Value as string);
                }
            }
        }

        private readonly IValueStorage[] storages;

        public AnyWhereConfigurationEnvironment()
        {
            this.storages = new IValueStorage[]
            {
                new ProcessEnvironmentValueStorage()
            };
        }

        public T GetValue<T>(
            string name,
            Func<string, (T value, bool converted)> convertFunc,
            T defaultValue = default(T),
            bool optional = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            string value = null;
            foreach (var storage in this.storages)
            {
                value = storage.Get(name);
                if (value != null)
                {
                    break;
                }
            }

            if (value == null)
            {
                if (!optional)
                {
                    throw new InvalidOperationException($"The '{name}' isn't found.");
                }

                return defaultValue;
            }

            var result = convertFunc(value);
            if (!result.converted)
            {
                throw new InvalidCastException($"The '{name}' values '{value}' cannot be converted to {nameof(T)}.");
            }

            return result.value;
        }

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            var set = new HashSet<string>();

            foreach (var storage in this.storages.Reverse())
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