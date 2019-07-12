using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationAdapterArguments
    {
        IAnyWhereConfigurationEnvironment Environment { get; }

        IEnumerable<AnyWhereConfigurationAdapterArgument> Enumerate();
    }
}