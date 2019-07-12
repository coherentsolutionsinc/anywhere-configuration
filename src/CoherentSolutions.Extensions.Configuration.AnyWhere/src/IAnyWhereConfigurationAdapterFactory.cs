using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationAdapterFactory
    {
        IEnumerable<AnyWhereConfigurationAdapterProxy> CreateProxies(
            IAnyWhereConfigurationAdapterArguments adapterArguments);
    }
}