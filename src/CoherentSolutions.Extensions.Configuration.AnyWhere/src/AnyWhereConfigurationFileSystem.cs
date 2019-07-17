using System;
using System.IO;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationFileSystem : IAnyWhereConfigurationFileSystem
    {
        public bool FileExists(
            string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            return File.Exists(path);
        }

        public string GetFileContentAsString(
            string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            return File.ReadAllText(path);
        }
    }
}