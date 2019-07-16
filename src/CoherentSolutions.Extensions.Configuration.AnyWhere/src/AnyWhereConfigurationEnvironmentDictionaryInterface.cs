using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationEnvironmentDictionaryInterface : IReadOnlyDictionary<string, string>
    {
        private readonly IAnyWhereConfigurationEnvironment environment;

        private IReadOnlyDictionary<string, string> values;

        public int Count
        {
            get
            {
                this.EnsureValues();
                return this.values.Count;
            }
        }

        public string this[
            string key]
        {
            get
            {
                this.EnsureValues();
                return this.values[key];
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                this.EnsureValues();
                return this.values.Keys;
            }
        }

        public IEnumerable<string> Values
        {
            get
            {
                this.EnsureValues();
                return this.values.Values;
            }
        }

        public AnyWhereConfigurationEnvironmentDictionaryInterface(
            IAnyWhereConfigurationEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.values = null;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            this.EnsureValues();
            return this.values.GetEnumerator();
        }

        public bool ContainsKey(
            string key)
        {
            if (this.values != null)
            {
                return this.values.ContainsKey(key);
            }

            return this.environment.GetValue(key, s => (s, true), null, true) != null;
        }

        public bool TryGetValue(
            string key,
            out string value)
        {
            if (this.values != null)
            {
                return this.values.TryGetValue(key, out value);
            }

            value = this.environment.GetValue(key, s => (s, true), null, true);
            return value != null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void EnsureValues()
        {
            if (this.values is null)
            {
                this.values = this.environment.GetValues().ToDictionary(kv => kv.Key, kv => kv.Value);
            }
        }
    }
}