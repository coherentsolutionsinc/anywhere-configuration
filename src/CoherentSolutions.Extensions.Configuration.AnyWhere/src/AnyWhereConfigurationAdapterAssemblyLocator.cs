using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterAssemblyLocator : IAnyWhereConfigurationAdapterAssemblyLocator
    {
        private const string ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH_PARAMETER_NAME = "PROBING_PATH";

        private readonly IEnumerable<string> probingPaths;

        public AnyWhereConfigurationAdapterAssemblyLocator(
            IAnyWhereConfigurationEnvironment environment)
        {
            IEnumerable<string> EnumerateProbingPaths()
            {
                yield return Directory.GetCurrentDirectory();

                var environmentReader = new AnyWhereConfigurationEnvironmentReader(environment);
                var paths = environmentReader
                   .GetString(ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH_PARAMETER_NAME, string.Empty, optional: true)
                   .Split(
                        new[]
                        {
                            Path.PathSeparator
                        },
                        StringSplitOptions.RemoveEmptyEntries);

                foreach (var path in paths)
                {
                    if (!Path.IsPathRooted(path))
                    {
                        yield return Path.GetFullPath(path);
                    }

                    yield return path;
                }
            }

            if (environment is null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            this.probingPaths = EnumerateProbingPaths().ToArray();
        }

        public IEnumerable<string> GetProbingPaths()
        {
            return this.probingPaths;
        }

        public string FindAssembly(
            string assemblyName)
        {
            var dll = $"{assemblyName}.dll";
            var exe = $"{assemblyName}.exe";

            foreach (var probingPath in this.probingPaths)
            {
                var path = Path.Combine(probingPath, dll);
                if (File.Exists(path))
                {
                    return path;
                }

                path = Path.Combine(probingPath, exe);
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }
    }
}