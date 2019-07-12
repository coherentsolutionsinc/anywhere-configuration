using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationPaths : IAnyWhereConfigurationPaths
    {
        private readonly IAnyWhereConfigurationEnvironment environment;

        private readonly string[] parameters;

        public AnyWhereConfigurationPaths(
            IAnyWhereConfigurationEnvironment environment,
            params string[] parameters)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.parameters = parameters;
        }

        public IEnumerable<AnyWhereConfigurationPath> Enumerate()
        {
            if (this.parameters.Length == 0)
            {
                return Enumerable.Empty<AnyWhereConfigurationPath>();
            }

            var output = new List<AnyWhereConfigurationPath>(1);

            var environmentReader = new AnyWhereConfigurationEnvironmentReader(this.environment);
            foreach (var p in this.parameters)
            {
                foreach (var arg in new AnyWhereConfigurationPathEnumerable(environmentReader.GetString(p)))
                {
                    if (Directory.Exists(arg.Value))
                    {
                        output.Add(arg);
                    }
                }
            }

            return output;
        }
    }
}