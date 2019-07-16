using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationEnvironmentSource
    {
        string Get(
            string name);

        IEnumerable<KeyValuePair<string, string>> GetValues();
    }
}