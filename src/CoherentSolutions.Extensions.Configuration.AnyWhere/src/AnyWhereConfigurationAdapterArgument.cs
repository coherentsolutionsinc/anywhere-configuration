using System;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationAdapterArgument
    {
        public AnyWhereConfigurationAdapterDefinition Definition { get; }

        public IAnyWhereConfigurationEnvironment Environment { get; }

        public AnyWhereConfigurationAdapterArgument(
            AnyWhereConfigurationAdapterDefinition definition,
            IAnyWhereConfigurationEnvironment environment)
        {
            this.Definition = definition;
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }
    }
}