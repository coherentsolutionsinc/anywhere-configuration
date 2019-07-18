using System;
using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationEnvironmentWithExtension : IAnyWhereConfigurationEnvironment
    {
        private readonly IAnyWhereConfigurationEnvironment source;
        private readonly IAnyWhereConfigurationEnvironment extension;

        public AnyWhereConfigurationEnvironmentWithExtension(
            IAnyWhereConfigurationEnvironment source,
            IAnyWhereConfigurationEnvironment extension)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.extension = extension ?? throw new ArgumentNullException(nameof(extension));
        }

        public T GetValue<T>(
            string name,
            Func<string, (T value, bool converted)> convertFunc,
            T defaultValue = default,
            bool optional = false)
        {
            var value = this.extension.GetValue(name, convertFunc, default, true);
            return EqualityComparer<T>.Default.Equals(value, default)
                ? this.source.GetValue(name, convertFunc, defaultValue, optional)
                : value;
        }

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            var set = new HashSet<string>();

            foreach (var kv in this.extension.GetValues().Concat(this.source.GetValues()))
            {
                if (set.Add(kv.Key))
                {
                    yield return kv;
                }
            }
        }
    }
}