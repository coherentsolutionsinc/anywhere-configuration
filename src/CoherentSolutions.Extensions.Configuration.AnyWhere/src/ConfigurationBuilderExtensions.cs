using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public static class ConfigurationBuilderExtensions
    {
        private const string ANYWHERE_ADAPTER_LIST_PROPERTY = "coherentsolutions.anywhere.adapterlist";

        private const string ANYWHERE_ADAPTER_PARAMETER_PREFIX = "ANYWHERE_ADAPTER";

        public static IConfigurationBuilder AddAnyWhereConfiguration(
            this IConfigurationBuilder configurationBuilder)
        {
            var adapterList =
                configurationBuilder.Properties[ANYWHERE_ADAPTER_LIST_PROPERTY] as IEnumerable<(string adapterName, string typeName, string assemblyPath)>;

            if (adapterList == null)
            {
                adapterList = Enumerable.Empty<(string adapterName, string typeName, string assemblyPath)>();
            }

            var configurationLoaderEnvironment =
                new AnyWhereConfigurationEnvironmentWithPrefix(
                    new AnyWhereConfigurationEnvironment(),
                    ANYWHERE_ADAPTER_PARAMETER_PREFIX);

            var configurationLoader = new AnyWhereConfigurationLoader(
                new AnyWhereConfigurationLoaderAdapterArgumentsReader(
                    adapterList.ToDictionary(
                        v => v.adapterName,
                        v => new AnyWhereConfigurationLoaderAdapterMetadata(v.adapterName, v.typeName, v.assemblyPath))),
                new AnyWhereConfigurationLoaderAssemblyLocator(),
                new AnyWhereConfigurationLoaderTypeLoader());

            configurationLoader.ConfigureAppConfiguration(
                configurationBuilder,
                configurationLoaderEnvironment);

            return configurationBuilder;
        }
    }
}