using System;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationAdapterArgumentEnumerable
    {
        private readonly IAnyWhereConfigurationEnvironment environment;

        private readonly IReadOnlyDictionary<string, AnyWhereConfigurationAdapterDefinition> knownAdapters;

        public AnyWhereConfigurationAdapterArgumentEnumerable(
            IAnyWhereConfigurationEnvironment environment,
            IReadOnlyDictionary<string, AnyWhereConfigurationAdapterDefinition> knownAdapters)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.knownAdapters = knownAdapters ?? throw new ArgumentNullException(nameof(knownAdapters));
        }

        public AnyWhereConfigurationAdapterArgumentEnumerator GetEnumerator()
        {
            return new AnyWhereConfigurationAdapterArgumentEnumerator(
                this.environment,
                this.knownAdapters);
        }
    }
}