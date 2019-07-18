using System;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationAdapterProbingPathEnumerable
    {
        private readonly IAnyWhereConfigurationEnvironment environment;

        public AnyWhereConfigurationAdapterProbingPathEnumerable(
            IAnyWhereConfigurationEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public AnyWhereConfigurationAdapterProbingPathEnumerator GetEnumerator()
        {
            return new AnyWhereConfigurationAdapterProbingPathEnumerator(this.environment);
        }
    }
}