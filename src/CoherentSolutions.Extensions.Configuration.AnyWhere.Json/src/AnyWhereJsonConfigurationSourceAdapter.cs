using System;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Json
{
    public class AnyWhereJsonConfigurationSourceAdapter : IAnyWhereConfigurationSourceAdapter
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

            configurationBuilder.AddJsonFile(
                environmentReader.GetString("PATH"),
                environmentReader.GetBool("OPTIONAL", optional: true),
                environmentReader.GetBool("RELOAD_ON_CHANGE", optional: true));
        }
    }
}