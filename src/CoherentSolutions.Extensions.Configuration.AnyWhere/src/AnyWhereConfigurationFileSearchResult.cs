using System;
using System.Collections.Generic;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationFileSearchResult : IAnyWhereConfigurationFileSearchResult
    {
        public string Directory { get; }

        public IReadOnlyList<IAnyWhereConfigurationFile> Files { get; }

        public AnyWhereConfigurationFileSearchResult(
            string directory,
            IReadOnlyList<IAnyWhereConfigurationFile> files)
        {
            this.Directory = directory ?? throw new ArgumentNullException(nameof(directory));
            this.Files = files ?? throw new ArgumentNullException(nameof(files));
        }
    }
}