using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationLoaderAdapterArgumentsEnumerator : IEnumerator<AnyWhereConfigurationLoaderAdapterArgument>
    {
        public const string ANYWHERE_ADAPTER_NAME_PARAMETER_NAME = "NAME";

        public const string ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME = "TYPE_NAME";

        public const string ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME = "ASSEMBLY_NAME";

        public const string ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME = "SEARCH_PATHS";

        private readonly IReadOnlyDictionary<string, AnyWhereConfigurationLoaderAdapterMetadata> adaptersMetadata;

        private readonly IAnyWhereConfigurationEnvironment environment;

        private AnyWhereConfigurationLoaderAdapterArgument current;

        private bool completed;

        private int index;

        public AnyWhereConfigurationLoaderAdapterArgument Current
        {
            get
            {
                if (this.index == 0)
                {
                    throw new InvalidOperationException("The enumeration isn't started.");
                }

                return this.current;
            }
        }

        object IEnumerator.Current => this.Current;

        public AnyWhereConfigurationLoaderAdapterArgumentsEnumerator(
            IReadOnlyDictionary<string, AnyWhereConfigurationLoaderAdapterMetadata> adaptersMetadata,
            IAnyWhereConfigurationEnvironment environment)
        {
            this.adaptersMetadata = adaptersMetadata ?? throw new ArgumentNullException(nameof(adaptersMetadata));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));

            this.completed = false;
            this.index = 0;
        }

        public bool MoveNext()
        {
            if (this.completed)
            {
                return false;
            }

            var adapterEnvironmentReader = new AnyWhereConfigurationEnvironmentReader(
                new AnyWhereConfigurationEnvironmentWithPrefix(
                    this.environment,
                    this.index.ToString()));

            string adapterTypeName = null;
            string adapterAssemblyName = null;

            var adapterName = adapterEnvironmentReader.GetString(ANYWHERE_ADAPTER_NAME_PARAMETER_NAME, optional: true);
            if (adapterName != null)
            {
                if (!this.adaptersMetadata.TryGetValue(adapterName, out var adapterMetadata))
                {
                    throw new InvalidOperationException($"There is no adapter with name '{adapterName}'.");
                }

                adapterTypeName = adapterMetadata.TypeName;
                adapterAssemblyName = adapterMetadata.AssemblyName;
            }
            else
            {
                adapterTypeName = adapterEnvironmentReader.GetString(ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME, optional: true);
                adapterAssemblyName = adapterEnvironmentReader.GetString(ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME, optional: true);

                if (adapterTypeName == null && adapterAssemblyName == null)
                {
                    this.current = default(AnyWhereConfigurationLoaderAdapterArgument);
                    this.completed = true;
                    return false;
                }

                adapterTypeName = adapterEnvironmentReader.GetString(ANYWHERE_ADAPTER_TYPE_NAME_PARAMETER_NAME);
                adapterAssemblyName = adapterEnvironmentReader.GetString(ANYWHERE_ADAPTER_ASSEMBLY_NAME_PARAMETER_NAME);
            }

            var adapterSearchPaths =
                adapterEnvironmentReader.GetString(ANYWHERE_ADAPTER_SEARCH_PATHS_PARAMETER_NAME, optional: true) ?? Directory.GetCurrentDirectory();

            this.index++;
            this.current = new AnyWhereConfigurationLoaderAdapterArgument(
                adapterEnvironmentReader,
                adapterTypeName,
                adapterAssemblyName,
                adapterSearchPaths);

            return true;
        }

        public void Reset()
        {
            this.index = 0;
            this.completed = false;
        }

        public void Dispose()
        {
            this.index = 0;
            this.completed = false;
        }
    }
}