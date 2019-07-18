using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationFile : IAnyWhereConfigurationFile
    {
        private readonly IAnyWhereConfigurationFileSystem fs;

        public string Name { get; }

        public string Directory { get; }

        public string Path { get; }

        public AnyWhereConfigurationFile(
            IAnyWhereConfigurationFileSystem fs,
            string name,
            string directory,
            string path)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(directory));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            this.fs = fs ?? throw new ArgumentNullException(nameof(fs));

            this.Name = name;
            this.Directory = directory;
            this.Path = path;
        }

        public string GetContentAsString()
        {
            return this.fs.GetFileContentAsString(this.Path);
        }
    }
}