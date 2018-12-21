using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationLoaderAdapterArgumentsReader
    {
        IEnumerable<AnyWhereConfigurationLoaderAdapterArgument> Read(
            IAnyWhereConfigurationEnvironment environment);
    }
}