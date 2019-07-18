using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationFileSearch
    {
        IReadOnlyList<IAnyWhereConfigurationFileSearchResult> Find(
            IReadOnlyCollection<string> directories,
            string name,
            params string[] extensions);

        IReadOnlyList<IAnyWhereConfigurationFile> Find(
            string directory,
            string name,
            params string[] extensions);
    }
}