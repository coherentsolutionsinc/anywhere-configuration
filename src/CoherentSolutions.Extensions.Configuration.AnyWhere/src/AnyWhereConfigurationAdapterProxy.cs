using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationAdapterProxy
    {
        private readonly IAnyWhereConfigurationAdapter adapter;

        private readonly IAnyWhereConfigurationEnvironment adapterEnvironment;

        private readonly AnyWhereConfigurationAdapterDefinition adapterDefinition;

        public AnyWhereConfigurationAdapterProxy(
            IAnyWhereConfigurationAdapter adapter,
            IAnyWhereConfigurationEnvironment adapterEnvironment,
            AnyWhereConfigurationAdapterDefinition adapterDefinition)
        {
            this.adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));

            this.adapterEnvironment = adapterEnvironment ?? throw new ArgumentNullException(nameof(adapterEnvironment));
            this.adapterDefinition = adapterDefinition;
        }

        public void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder)
        {
            if (configurationBuilder is null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            try
            {
                this.adapter.ConfigureAppConfiguration(
                    configurationBuilder,
                    new AnyWhereConfigurationEnvironmentReader(this.adapterEnvironment));
            }
            catch (Exception e)
            {
                var sb = new StringBuilder()
                   .AppendFormat($"Could not apply adapter's '{this.adapterDefinition.TypeName}' configuration.")
                   .AppendLine();

                var arguments = this.adapterEnvironment.GetValues().ToDictionary(kv => kv.Key, kv => kv.Value);
                if (arguments.Count != 0)
                {
                    sb.AppendFormat("The following arguments ({0} in total) were provided:", arguments.Count)
                       .AppendLine();

                    foreach (var kv in arguments)
                    {
                        sb.AppendFormat("'{0}'='{1}'", kv.Key, kv.Value)
                           .AppendLine();
                    }
                }

                throw new InvalidOperationException(
                    sb.ToString(),
                    ExceptionDispatchInfo.Capture(e).SourceException);
            }
        }
    }
}