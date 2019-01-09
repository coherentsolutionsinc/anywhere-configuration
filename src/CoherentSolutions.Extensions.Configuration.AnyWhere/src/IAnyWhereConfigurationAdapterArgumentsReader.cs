using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationAdapterArgumentsReader
    {
        IEnumerable<AnyWhereConfigurationAdapterArgument> Read(
            IAnyWhereConfigurationEnvironment environment);
    }
}