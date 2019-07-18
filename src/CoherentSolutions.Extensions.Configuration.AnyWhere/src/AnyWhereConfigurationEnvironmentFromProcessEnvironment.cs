using System;
using System.Collections;
using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationEnvironmentFromProcessEnvironment : IAnyWhereConfigurationEnvironmentSource
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
}