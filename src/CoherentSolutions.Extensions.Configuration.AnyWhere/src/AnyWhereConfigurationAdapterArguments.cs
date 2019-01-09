using System;
using System.Collections;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterArguments : IEnumerable<AnyWhereConfigurationAdapterArgument>
    {
        private readonly IReadOnlyDictionary<string, AnyWhereConfigurationAdapterMetadata> adaptersMetadata;

        private readonly IAnyWhereConfigurationEnvironment environment;

        public AnyWhereConfigurationAdapterArguments(
            IReadOnlyDictionary<string, AnyWhereConfigurationAdapterMetadata> adaptersMetadata,
            IAnyWhereConfigurationEnvironment environment)
        {
            this.adaptersMetadata = adaptersMetadata ?? throw new ArgumentNullException(nameof(adaptersMetadata));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IEnumerator<AnyWhereConfigurationAdapterArgument> GetEnumerator()
        {
            return new AnyWhereConfigurationAdapterArgumentsEnumerator(
                this.adaptersMetadata,
                this.environment);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}