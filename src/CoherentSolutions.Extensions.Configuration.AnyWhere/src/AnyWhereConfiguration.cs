using System;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfiguration
    {
        private readonly IAnyWhereConfigurationAdapterArguments adapterArguments;

        private readonly IAnyWhereConfigurationAdapterFactory adapterFactory;

        public AnyWhereConfiguration(
            IAnyWhereConfigurationAdapterArguments adapterArguments,
            IAnyWhereConfigurationAdapterFactory adapterFactory)
        {
            this.adapterArguments = adapterArguments ?? throw new ArgumentNullException(nameof(adapterArguments));
            this.adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
        }

        public void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder)
        {
            if (configurationBuilder is null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            foreach (var adapterProxy in this.adapterFactory.CreateProxies(this.adapterArguments))
            {
                adapterProxy.ConfigureAppConfiguration(configurationBuilder);
            }
        }
    }
}