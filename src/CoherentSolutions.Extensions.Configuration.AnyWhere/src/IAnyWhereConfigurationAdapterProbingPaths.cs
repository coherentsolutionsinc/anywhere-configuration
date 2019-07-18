using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationAdapterProbingPaths
    {
        IEnumerable<AnyWhereConfigurationDataPath> Enumerate();
    }
}