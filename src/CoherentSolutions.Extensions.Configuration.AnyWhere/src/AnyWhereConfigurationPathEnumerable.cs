using System;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationPathEnumerable
    {
        private readonly IAnyWhereConfigurationEnvironment environment;


        public AnyWhereConfigurationPathEnumerable(
            IAnyWhereConfigurationEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public AnyWhereConfigurationPathEnumerator GetEnumerator()
        {
            return new AnyWhereConfigurationPathEnumerator(this.environment);
        }
    }
}