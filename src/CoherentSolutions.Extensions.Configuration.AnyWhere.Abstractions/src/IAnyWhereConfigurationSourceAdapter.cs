using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions
{
    public interface IAnyWhereConfigurationSourceAdapter
    {
        void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder,
            IAnyWhereConfigurationEnvironmentReader environmentReader);
    }
}