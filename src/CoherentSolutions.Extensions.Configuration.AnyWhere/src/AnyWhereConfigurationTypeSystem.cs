using System;
using System.Reflection;
using System.Runtime.Loader;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationTypeSystem : IAnyWhereConfigurationTypeSystem
    {
        private class TypeSystemLoadContext : AssemblyLoadContext
        {
            private const int EXE_EXTENSION = 0;

            private const int DLL_EXTENSION = 1;

            private static readonly string[] extensions = new[]
            {
                ".exe",
                ".dll"
            };

            private readonly IAnyWhereConfigurationFileSearch search;

            private readonly string basePath;

            public TypeSystemLoadContext(
                string basePath,
                IAnyWhereConfigurationFileSearch search)
            {
                if (string.IsNullOrWhiteSpace(basePath))
                {
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(basePath));
                }

                this.basePath = basePath;
                this.search = search ?? throw new ArgumentNullException(nameof(search));
            }

            protected override Assembly Load(
                AssemblyName assemblyName)
            {
                try
                {
                    return Default.LoadFromAssemblyName(assemblyName);
                }
                catch
                {
                    var results = this.search.Find(this.basePath, assemblyName.Name, extensions);
                    if (results is null || results.Count == 0)
                    {
                        return null;
                    }

                    var assembly = results[EXE_EXTENSION] ?? results[DLL_EXTENSION];
                    return !AssemblyName.ReferenceMatchesDefinition(assemblyName, GetAssemblyName(assembly.Path))
                        ? null
                        : this.LoadFromAssemblyPath(assembly.Path);
                }
            }
        }

        private readonly IAnyWhereConfigurationFileSearch search;

        public AnyWhereConfigurationTypeSystem(
            IAnyWhereConfigurationFileSearch search)
        {
            this.search = search ?? throw new ArgumentNullException(nameof(search));
        }

        public IAnyWhereConfigurationType Get(
            IAnyWhereConfigurationFile assembly,
            string name)
        {
            var lc = new TypeSystemLoadContext(assembly.Directory, this.search);
            try
            {
                return new AnyWhereConfigurationType(
                    lc.LoadFromAssemblyPath(assembly.Path).GetType(name, true));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    $"Could not load assembly '{assembly.Path}' or it's dependencies. Please see inner exception for details.",
                    e);
            }
        }
    }
}