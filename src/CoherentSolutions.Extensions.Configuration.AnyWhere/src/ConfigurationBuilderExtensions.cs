using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public static class ConfigurationBuilderExtensions
    {
        private const string ANYWHERE_ADAPTER_LIST_PROPERTY = "coherentsolutions.anywhere.adapterlist";

        private const string ANYWHERE_ADAPTER_PARAMETER_PREFIX = "ANYWHERE_ADAPTER";

        private const string ANYWHERE_ADAPTER_GLOBAL_PARAMETER_PREFIX = "ANYWHERE_ADAPTER_GLOBAL";

        public static IConfigurationBuilder AddAnyWhereConfiguration(
            this IConfigurationBuilder configurationBuilder)
        {
            var adapterList = Enumerable.Empty<(string adapterName, string typeName, string assemblyPath)>();
            if (configurationBuilder.Properties.TryGetValue(ANYWHERE_ADAPTER_LIST_PROPERTY, out var propertyValue))
            {
                adapterList = (IEnumerable<(string adapterName, string typeName, string assemblyPath)>)propertyValue;
            }

            var environment = new AnyWhereConfigurationEnvironment();

            var adapterConfigurationEnvironment =
                new AnyWhereConfigurationEnvironmentWithPrefix(
                    environment,
                    ANYWHERE_ADAPTER_PARAMETER_PREFIX);

            var adapterGlobalConfigurationEnvironment = 
                new AnyWhereConfigurationEnvironmentWithPrefix(
                    environment,
                    ANYWHERE_ADAPTER_GLOBAL_PARAMETER_PREFIX);

            var configuration = new AnyWhereConfiguration(
                new AnyWhereConfigurationAdapterArgumentsReader(
                    adapterList.ToDictionary(
                        v => v.adapterName,
                        v => new AnyWhereConfigurationAdapterMetadata(v.adapterName, v.typeName, v.assemblyPath))),
                new AnyWhereConfigurationAdapterFactory(
                    new AnyWhereConfigurationAdapterFactoryTypeLoader(
                        new AnyWhereConfigurationAdapterAssemblyLocator(adapterGlobalConfigurationEnvironment))));

            configuration.ConfigureAppConfiguration(
                configurationBuilder,
                adapterConfigurationEnvironment);

            return configurationBuilder;
        }
    }
}