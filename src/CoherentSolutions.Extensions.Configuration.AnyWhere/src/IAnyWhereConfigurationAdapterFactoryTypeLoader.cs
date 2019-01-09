using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationAdapterFactoryTypeLoader
    {
        Type Load(
            AnyWhereConfigurationAdapterArgument adapterArg);
    }
}