using System;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile
{
    public class AnyWhereKeyPerFileConfigurationSourceAdapter : IAnyWhereConfigurationAdapter
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

            configurationBuilder.AddKeyPerFile(
                environmentReader.GetString("DIRECTORY_PATH"),
                environmentReader.GetBool("OPTIONAL", optional: true));
        }
    }
}