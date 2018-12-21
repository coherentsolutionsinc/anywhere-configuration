using System;
using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions
{
    public interface IAnyWhereConfigurationEnvironment
    {
        T GetValue<T>(
            string name,
            Func<string, (T value, bool converted)> convertFunc,
            T defaultValue = default(T),
            bool optional = false);

        IEnumerable<KeyValuePair<string, string>> GetValues();
    }
}