using System;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterArgumentsReader : IAnyWhereConfigurationAdapterArgumentsReader
    {
        private readonly IReadOnlyDictionary<string, AnyWhereConfigurationAdapterMetadata> adaptersMetadata;

        public AnyWhereConfigurationAdapterArgumentsReader(
            IReadOnlyDictionary<string, AnyWhereConfigurationAdapterMetadata> adaptersMetadata)
        {
            this.adaptersMetadata = adaptersMetadata ?? throw new ArgumentNullException(nameof(adaptersMetadata));
        }

        public IEnumerable<AnyWhereConfigurationAdapterArgument> Read(
            IAnyWhereConfigurationEnvironment environment)
        {
            return new AnyWhereConfigurationAdapterArguments(
                this.adaptersMetadata,
                environment);
        }
    }
}