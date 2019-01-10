using System;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables
{
    public class AnyWhereEnvironmentVariablesConfigurationSourceAdapter : IAnyWhereConfigurationAdapter
    {
        public void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder,
            IAnyWhereConfigurationEnvironmentReader environmentReader)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            if (environmentReader == null)
            {
                throw new ArgumentNullException(nameof(environmentReader));
            }

            configurationBuilder.AddEnvironmentVariables(
                environmentReader.GetString("PREFIX", string.Empty, true));
        }
    }
}