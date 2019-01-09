using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationAdapterArgument
    {
        public IAnyWhereConfigurationEnvironmentReader AdapterEnvironmentReader { get; }

        public string AdapterTypeName { get; }

        public string AdapterAssemblyName { get; }

        public AnyWhereConfigurationAdapterArgument(
            IAnyWhereConfigurationEnvironmentReader adapterEnvironmentReader,
            string adapterTypeName,
            string adapterAssemblyName)
        {
            this.AdapterEnvironmentReader = adapterEnvironmentReader;
            this.AdapterTypeName = adapterTypeName;
            this.AdapterAssemblyName = adapterAssemblyName;
        }
    }
}