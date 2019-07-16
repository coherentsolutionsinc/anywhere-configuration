using System;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterArguments : IAnyWhereConfigurationAdapterArguments
    {
        private readonly IAnyWhereConfigurationEnvironment environment;

        private readonly IReadOnlyDictionary<string, AnyWhereConfigurationAdapterDefinition> knownAdapters;

        public AnyWhereConfigurationAdapterArguments(
            IAnyWhereConfigurationEnvironment environment,
            IReadOnlyDictionary<string, AnyWhereConfigurationAdapterDefinition> knownAdapters)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.knownAdapters = knownAdapters ?? throw new ArgumentNullException(nameof(knownAdapters));
        }

        public IEnumerable<AnyWhereConfigurationAdapterArgument> Enumerate()
        {
            foreach (var arg in new AnyWhereConfigurationAdapterArgumentEnumerable(this.environment, this.knownAdapters))
            {
                yield return arg;
            }
        }
    }
}