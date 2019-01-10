using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions
{
    public interface IAnyWhereConfigurationAdapter
    {
        void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder,
            IAnyWhereConfigurationEnvironmentReader environmentReader);
    }
}