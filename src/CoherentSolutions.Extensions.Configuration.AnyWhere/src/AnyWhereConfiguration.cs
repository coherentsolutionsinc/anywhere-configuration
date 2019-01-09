using System;
using System.Linq;
using System.Runtime.ExceptionServices;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfiguration
    {
        private readonly IAnyWhereConfigurationAdapterArgumentsReader adapterArgumentsReader;

        private readonly IAnyWhereConfigurationAdapterFactory adapterFactory;

        public AnyWhereConfiguration(
            IAnyWhereConfigurationAdapterArgumentsReader adapterArgumentsReader,
            IAnyWhereConfigurationAdapterFactory adapterFactory)
        {
            this.adapterArgumentsReader = adapterArgumentsReader ?? throw new ArgumentNullException(nameof(adapterArgumentsReader));
            this.adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
        }

        public void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder,
            IAnyWhereConfigurationEnvironment environment)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            foreach (var adapterArg in this.adapterArgumentsReader.Read(environment))
            {
                try
                {
                    var adapter = this.adapterFactory.Create(adapterArg);

                    adapter.ConfigureAppConfiguration(
                        configurationBuilder,
                        adapterArg.AdapterEnvironmentReader);
                }
                catch (Exception exception)
                {
                    var arguments = adapterArg.AdapterEnvironmentReader.Environment.GetValues().ToDictionary(kv => kv.Key, kv => kv.Value);
                    throw new AnyWhereConfigurationException(
                        adapterArg.AdapterTypeName,
                        adapterArg.AdapterAssemblyName,
                        arguments,
                        ExceptionDispatchInfo.Capture(exception).SourceException);
                }
            }
        }
    }
}