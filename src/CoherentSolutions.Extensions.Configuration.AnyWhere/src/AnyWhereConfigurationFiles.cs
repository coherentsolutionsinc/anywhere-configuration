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

            if (this.directories.Count == 0)
            {
                return Array.Empty<string>();
            }

            var result = new string[extensions.Length == 0
                ? 1
                : extensions.Length];

            List<string> output = null;
            foreach (var directory in this.directories)
            {
                var changed = this.FindInternal(result, directory, name, extensions);
                if (changed == 0)
                {
                    continue;
                }

                if (output is null)
                {
                    output = new List<string>(result.Length + 1);
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

            var changed = this.FindInternal(result, directory, name, extensions);
            return changed == 0
                ? Array.Empty<string>()
                : result;
        }

        private int FindInternal(
            string[] result,
            string directory,
            string name,
            params string[] extensions)
        {
            var file = Path.Combine(directory, name);

            if (extensions.Length == 0)
            {
                if (this.fs.FileExists(file))
                {
                    result[0] = file;
                    return 1;
                }

                result[0] = null;
                return 0;
            }

            var count = 0;
            foreach (var (index, path) in extensions
               .Select((extension, index) => (index, path: Path.ChangeExtension(file, extension))))
            {
                if (this.fs.FileExists(path))
                {
                    result[index] = path;
                    count++;
                }
                else
                {
                    result[index] = null;
                }
            }

            return count;
        }
    }
}