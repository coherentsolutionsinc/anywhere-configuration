using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationAdapterAssemblyLocator
    {
        IEnumerable<string> GetProbingPaths();

        string FindAssembly(
            string assemblyName);
    }
}