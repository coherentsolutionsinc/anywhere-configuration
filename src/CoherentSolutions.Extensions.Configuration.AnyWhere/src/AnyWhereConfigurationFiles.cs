using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationFiles : IAnyWhereConfigurationFiles
    {
        private readonly IAnyWhereConfigurationFileSystem fs;

        private readonly HashSet<string> directories;

        public IEnumerable<string> Directories => this.directories;

        public AnyWhereConfigurationFiles(
            IAnyWhereConfigurationFileSystem fs,
            IAnyWhereConfigurationPaths paths)
        {
            this.fs = fs ?? throw new ArgumentNullException(nameof(fs));

            this.directories = new HashSet<string>();

            foreach (var path in paths.Enumerate())
            {
                this.directories.Add(path.Value);
            }
        }

        public IReadOnlyList<string> Find(
            string name,
            params string[] extensions)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            var output = new List<string>(
                extensions.Length == 0
                    ? 2
                    : extensions.Length + 1);

            var result = new string[extensions.Length == 0
                ? 1
                : extensions.Length];
            foreach (var directory in this.directories)
            {
                this.FindInternal(result, directory, name, extensions);

                if (result.All(i => i is null))
                {
                    continue;
                }

                output.AddRange(result);
                output.Add(directory);
            }

            return output;
        }

        public IReadOnlyList<string> Find(
            string directory,
            string name,
            params string[] extensions)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(directory));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (!this.directories.Contains(directory))
            {
                throw new InvalidOperationException($"Unknown '{directory}' directory.");
            }

            var result = new string[extensions.Length == 0
                ? 1
                : extensions.Length];

            this.FindInternal(result, directory, name, extensions);

            return result;
        }

        private void FindInternal(
            string[] result,
            string directory,
            string name,
            params string[] extensions)
        {
            var file = Path.Combine(directory, name);

            foreach (var (index, path) in extensions
               .Select(
                    (
                        extension,
                        index) => (index, path: Path.ChangeExtension(file, extension))))
            {
                result[index] = this.fs.FileExists(path)
                    ? path
                    : null;
            }
        }
    }
}