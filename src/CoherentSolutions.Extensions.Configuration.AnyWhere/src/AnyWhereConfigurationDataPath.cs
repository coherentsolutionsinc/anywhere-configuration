using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationDataPath
    {
        public readonly string Path;

        public AnyWhereConfigurationDataPath(
            in string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            this.Path = path;
        }
    }
}