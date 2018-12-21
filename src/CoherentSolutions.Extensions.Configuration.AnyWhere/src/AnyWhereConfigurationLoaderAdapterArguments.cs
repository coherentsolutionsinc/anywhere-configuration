using System;
using System.Collections;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationLoaderAdapterArguments : IEnumerable<AnyWhereConfigurationLoaderAdapterArgument>
    {
        private readonly IReadOnlyDictionary<string, AnyWhereConfigurationLoaderAdapterMetadata> adaptersMetadata;

        private readonly IAnyWhereConfigurationEnvironment environment;

        public AnyWhereConfigurationLoaderAdapterArguments(
            IReadOnlyDictionary<string, AnyWhereConfigurationLoaderAdapterMetadata> adaptersMetadata,
            IAnyWhereConfigurationEnvironment environment)
        {
            this.adaptersMetadata = adaptersMetadata ?? throw new ArgumentNullException(nameof(adaptersMetadata));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IEnumerator<AnyWhereConfigurationLoaderAdapterArgument> GetEnumerator()
        {
            return new AnyWhereConfigurationLoaderAdapterArgumentsEnumerator(
                this.adaptersMetadata,
                this.environment);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}