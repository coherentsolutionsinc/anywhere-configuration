using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationFileSearchResult
    {
        string Directory { get; }

        IReadOnlyList<IAnyWhereConfigurationFile> Files { get; }
    }
}