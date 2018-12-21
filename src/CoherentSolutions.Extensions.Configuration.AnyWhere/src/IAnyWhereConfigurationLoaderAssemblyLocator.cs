using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationLoaderAssemblyLocator
    {
        string FindAssembly(
            IReadOnlyCollection<string> locations,
            string name);
    }
}