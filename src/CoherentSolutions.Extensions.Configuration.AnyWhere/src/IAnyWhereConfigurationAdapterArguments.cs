using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationAdapterArguments
    {
        IEnumerable<AnyWhereConfigurationAdapterArgument> Enumerate();
    }
}