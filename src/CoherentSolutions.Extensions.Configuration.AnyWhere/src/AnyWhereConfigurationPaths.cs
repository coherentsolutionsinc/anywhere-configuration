using System;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationPaths : IAnyWhereConfigurationPaths
    {
        private readonly IAnyWhereConfigurationEnvironment environment;

        public AnyWhereConfigurationPaths(
            IAnyWhereConfigurationEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IEnumerable<AnyWhereConfigurationPath> Enumerate()
        {
            foreach (var path in new AnyWhereConfigurationPathEnumerable(this.environment))
            {
                yield return path;
            }
        }
    }
}