﻿using System.Collections.Generic;
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
            var adapterList = Enumerable.Empty<(string adapterName, string typeName, string assemblyName)>();
            if (configurationBuilder.Properties.TryGetValue(ANYWHERE_ADAPTER_LIST_PROPERTY, out var propertyValue))
            {
                adapterList = (IEnumerable<(string adapterName, string typeName, string assemblyPath)>) propertyValue;
            }

            var environment = new AnyWhereConfigurationEnvironment(
                new AnyWhereConfigurationEnvironmentFromProcessEnvironment());

            var configuration = new AnyWhereConfiguration(
                new AnyWhereConfigurationAdapterArguments(
                    new AnyWhereConfigurationEnvironmentWithPrefix(
                        environment,
                        ANYWHERE_ADAPTER_PARAMETER_PREFIX),
                    adapterList.ToDictionary(
                        v => v.adapterName,
                        v => new AnyWhereConfigurationAdapterDefinition(v.typeName, v.assemblyName))),
                new AnyWhereConfigurationAdapterProbingPaths(
                    new AnyWhereConfigurationEnvironmentWithPrefix(
                        environment,
                        ANYWHERE_ADAPTER_GLOBAL_PARAMETER_PREFIX)));

            configuration.ConfigureAppConfiguration(configurationBuilder);

            return configurationBuilder;
        }
    }
}