using System;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationLoaderAdapterArgumentsReader : IAnyWhereConfigurationLoaderAdapterArgumentsReader
    {
        private readonly IReadOnlyDictionary<string, AnyWhereConfigurationLoaderAdapterMetadata> adaptersMetadata;

        public AnyWhereConfigurationLoaderAdapterArgumentsReader(
            IReadOnlyDictionary<string, AnyWhereConfigurationLoaderAdapterMetadata> adaptersMetadata)
        {
            this.adaptersMetadata = adaptersMetadata ?? throw new ArgumentNullException(nameof(adaptersMetadata));
        }

        public IEnumerable<AnyWhereConfigurationLoaderAdapterArgument> Read(
            IAnyWhereConfigurationEnvironment environment)
        {
            return new AnyWhereConfigurationLoaderAdapterArguments(
                this.adaptersMetadata,
                environment);
        }
    }
}