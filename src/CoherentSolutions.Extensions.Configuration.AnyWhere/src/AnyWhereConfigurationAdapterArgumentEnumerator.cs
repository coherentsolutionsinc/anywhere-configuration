using System;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationAdapterArgumentEnumerator
    {
        public const string ANYWHERE_ADAPTER_NAME_PARAMETER_NAME = "NAME";

        public const string ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME = "TYPE_NAME";

        public const string ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME = "ASSEMBLY_NAME";

        private readonly IAnyWhereConfigurationEnvironment environment;

        private readonly IReadOnlyDictionary<string, AnyWhereConfigurationAdapterDefinition> knownAdapters;

        private AnyWhereConfigurationAdapterArgument current;

        private int index;

        private bool completed;

        public AnyWhereConfigurationAdapterArgument Current
        {
            get
            {
                if (this.index == 0)
                {
                    throw new InvalidOperationException("The enumeration wasn't started.");
                }

                return this.current;
            }
        }

        public AnyWhereConfigurationAdapterArgumentEnumerator(
            IAnyWhereConfigurationEnvironment environment,
            IReadOnlyDictionary<string, AnyWhereConfigurationAdapterDefinition> knownAdapters)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.knownAdapters = knownAdapters ?? throw new ArgumentNullException(nameof(knownAdapters));

            this.current = default;

            this.index = 0;
            this.completed = false;
        }

        public bool MoveNext()
        {
            if (this.completed)
            {
                return false;
            }

            var adapterKey = this.index.ToString();
            var adapterEnvironmentReader = new AnyWhereConfigurationEnvironmentReader(
                new AnyWhereConfigurationEnvironmentWithPrefix(
                    this.environment,
                    adapterKey));

            var adapterName = adapterEnvironmentReader.GetString(ANYWHERE_ADAPTER_NAME_PARAMETER_NAME, optional: true);
            if (adapterName is null)
            {
                var adapterTypeName = adapterEnvironmentReader.GetString(ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME, optional: true);
                var adapterAssemblyName = adapterEnvironmentReader.GetString(ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME, optional: true);

                if (adapterTypeName is null || adapterAssemblyName is null)
                {
                    this.current = default;
                    this.completed = true;
                    return false;
                }

                this.current = new AnyWhereConfigurationAdapterArgument(
                    new AnyWhereConfigurationAdapterDefinition(adapterTypeName, adapterAssemblyName),
                    adapterKey);
            }
            else
            {
                if (!this.knownAdapters.TryGetValue(adapterName, out var adapterDefinition))
                {
                    throw new InvalidOperationException($"There is no adapter with name '{adapterName}'.");
                }

                this.current = new AnyWhereConfigurationAdapterArgument(
                    adapterDefinition,
                    adapterKey);
            }

            this.index++;

            return true;
        }

        public void Reset()
        {
            this.index = 0;
            this.completed = false;
        }
    }
}