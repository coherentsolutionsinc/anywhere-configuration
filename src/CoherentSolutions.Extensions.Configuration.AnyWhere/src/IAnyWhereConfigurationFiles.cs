using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationFiles
    {
        IEnumerable<string> Directories { get; }

        IReadOnlyList<string> Find(
            string name,
            params string[] extensions);

        IReadOnlyList<string> Find(
            string directory,
            string name,
            params string[] extensions);
    }
}