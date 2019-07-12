﻿using System.IO;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationFileSystem : IAnyWhereConfigurationFileSystem
    {
        public bool FileExists(
            string path)
        {
            return File.Exists(path);
        }

        public bool DirectoryExists(
            string path)
        {
            return Directory.Exists(path);
        }
    }
}