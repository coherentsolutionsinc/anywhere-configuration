using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationAdapterFactory
    {
        IAnyWhereConfigurationAdapter Create(
            AnyWhereConfigurationAdapterArgument adapterArg);
    }
}