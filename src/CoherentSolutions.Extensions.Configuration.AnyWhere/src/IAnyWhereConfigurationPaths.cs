using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationPaths
    {
        IEnumerable<AnyWhereConfigurationPath> Enumerate();
    }
}