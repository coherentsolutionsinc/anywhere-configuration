using System;
using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationEnvironmentFromMemory : IAnyWhereConfigurationEnvironmentSource
    {
        private readonly IReadOnlyDictionary<string, string> values;

        public AnyWhereConfigurationEnvironmentFromMemory(
            IReadOnlyDictionary<string, string> values)
        {
            this.values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public string Get(
            string name)
        {
            return this.values.TryGetValue(name, out var output)
                ? output
                : null;
        }

        public IEnumerable<KeyValuePair<string, string>> GetValues()
        {
            return this.values;
        }
    }
}