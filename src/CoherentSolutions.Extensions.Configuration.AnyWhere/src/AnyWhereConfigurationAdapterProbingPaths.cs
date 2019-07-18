using System;
using System.Collections.Generic;
using System.IO;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterProbingPaths : IAnyWhereConfigurationAdapterProbingPaths
    {
        private readonly IAnyWhereConfigurationEnvironment environment;

        public AnyWhereConfigurationAdapterProbingPaths(
            IAnyWhereConfigurationEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IEnumerable<AnyWhereConfigurationDataPath> Enumerate()
        {
            yield return new AnyWhereConfigurationDataPath(Directory.GetCurrentDirectory());

            foreach (var path in new AnyWhereConfigurationAdapterProbingPathEnumerable(this.environment))
            {
                yield return path;
            }
        }
    }
}