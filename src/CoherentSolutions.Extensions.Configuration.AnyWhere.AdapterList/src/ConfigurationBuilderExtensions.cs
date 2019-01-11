using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList
{
    public static class ConfigurationBuilderExtensions
    {
        private const string ANYWHERE_ADAPTER_LIST_PROPERTY = "coherentsolutions.anywhere.adapterlist";

        public static IConfigurationBuilder AddAnyWhereConfigurationAdapterList(
            this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties[ANYWHERE_ADAPTER_LIST_PROPERTY] = new (string adapterName, string typeName, string assemblyName)[]
            {
                ("Json",
                    "CoherentSolutions.Extensions.Configuration.AnyWhere.Json.AnyWhereJsonConfigurationSourceAdapter",
                    "CoherentSolutions.Extensions.Configuration.AnyWhere.Json"),
                ("KeyPerFile",
                    "CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile.AnyWhereKeyPerFileConfigurationSourceAdapter",
                    "CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile"),
                ("EnvironmentVariables",
                    "CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables.AnyWhereEnvironmentVariablesConfigurationSourceAdapter",
                    "CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables")
            };

            return configurationBuilder;
        }
    }
}