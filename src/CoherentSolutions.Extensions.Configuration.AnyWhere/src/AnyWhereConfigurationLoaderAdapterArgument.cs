using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationLoaderAdapterArgument
    {
        public IAnyWhereConfigurationEnvironmentReader AdapterEnvironmentReader { get; }

        public string AdapterTypeName { get; }

        public string AdapterAssemblyName { get; }

        public string AdapterSearchPaths { get; }

        public AnyWhereConfigurationLoaderAdapterArgument(
            IAnyWhereConfigurationEnvironmentReader adapterEnvironmentReader,
            string adapterTypeName,
            string adapterAssemblyName,
            string adapterSearchPaths)
        {
            this.AdapterEnvironmentReader = adapterEnvironmentReader;
            this.AdapterTypeName = adapterTypeName;
            this.AdapterAssemblyName = adapterAssemblyName;
            this.AdapterSearchPaths = adapterSearchPaths;
        }
    }
}