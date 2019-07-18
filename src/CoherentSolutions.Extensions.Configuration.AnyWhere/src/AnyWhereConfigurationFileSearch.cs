using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationFileSearch : IAnyWhereConfigurationFileSearch
    {
        private readonly IAnyWhereConfigurationFileSystem fs;

        public AnyWhereConfigurationFileSearch(
            IAnyWhereConfigurationFileSystem fs)
        {
            this.fs = fs ?? throw new ArgumentNullException(nameof(fs));
        }

        public IReadOnlyList<IAnyWhereConfigurationFileSearchResult> Find(
            IReadOnlyCollection<string> directories,
            string name,
            params string[] extensions)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (directories.Count == 0)
            {
                return Array.Empty<IAnyWhereConfigurationFileSearchResult>();
            }

            var resultSz = extensions.Length == 0
                ? 1
                : extensions.Length;

            IAnyWhereConfigurationFile[] result = null;
            List<IAnyWhereConfigurationFileSearchResult> output = null;
            foreach (var directory in directories)
            {
                if (result is null)
                {
                    result = new IAnyWhereConfigurationFile[resultSz];
                }

                var changed = this.FindInternal(result, directory, name, extensions);
                if (changed == 0)
                {
                    continue;
                }

                if (output is null)
                {
                    output = new List<IAnyWhereConfigurationFileSearchResult>(1);
                }

                output.Add(
                    new AnyWhereConfigurationFileSearchResult(
                        directory, 
                        result));

                result = null;
            }

            return output;
        }

        public IReadOnlyList<IAnyWhereConfigurationFile> Find(
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

            var result = new IAnyWhereConfigurationFile[extensions.Length == 0
                ? 1
                : extensions.Length];

            var changed = this.FindInternal(result, directory, name, extensions);
            return changed == 0
                ? Array.Empty<IAnyWhereConfigurationFile>()
                : result;
        }

        private int FindInternal(
            IList<IAnyWhereConfigurationFile> result,
            string directory,
            string name,
            params string[] extensions)
        {

            if (extensions.Length == 0)
            {
                var path = Path.Combine(directory, name);
                if (this.fs.FileExists(path))
                {
                    result[0] = new AnyWhereConfigurationFile(this.fs, name, directory, path);
                    return 1;
                }

                result[0] = null;
                return 0;
            }

            var count = 0;
            foreach (var (index, file) in extensions
               .Select((extension, index) => (index, path: string.Concat(name, extension))))
            {
                var path = Path.Combine(directory, file);
                if (this.fs.FileExists(path))
                {
                    result[index] = new AnyWhereConfigurationFile(this.fs, name, directory, path);
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