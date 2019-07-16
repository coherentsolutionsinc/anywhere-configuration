using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Loader;
using System.Text;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterFactory : IAnyWhereConfigurationAdapterFactory
    {
        private class AdapterAssemblyLoadContext : AssemblyLoadContext
        {
            private static readonly string[] extensions =
            {
                ".exe",
                ".dll"
            };

            private readonly string directory;

            private readonly IAnyWhereConfigurationFiles files;

            public AdapterAssemblyLoadContext(
                string directory,
                IAnyWhereConfigurationFiles files)
            {
                this.directory = directory ?? throw new ArgumentNullException(nameof(directory));
                this.files = files ?? throw new ArgumentNullException(nameof(files));
            }

            protected override Assembly Load(
                AssemblyName assemblyName)
            {
                try
                {
                    // We always prefer assemblies from the default context over the adapter assembly dependency.
                    return Default.LoadFromAssemblyName(assemblyName);
                }
                catch
                {
                    // There is no 'try' overload for assembly loading, if assembly cannot be loaded then we need
                    // to find it manually.
                    // Here we are configuring files to search only in same directory where main assembly was found.
                    var result = this.files.Find(this.directory, assemblyName.Name, extensions);
                    if (result.Count == 0)
                    {
                        return null;
                    }

                    var candidateAssemblyPath = result[EXE_EXTENSION] ?? result[DLL_EXTENSION];
                    if (candidateAssemblyPath is null)
                    {
                        return null;
                    }

                    var candidateAssemblyName = GetAssemblyName(candidateAssemblyPath);
                    return !AssemblyName.ReferenceMatchesDefinition(assemblyName, candidateAssemblyName)
                        ? null
                        : this.LoadFromAssemblyPath(candidateAssemblyPath);
                }
            }
        }

        private const int EXE_EXTENSION = 0;

        private const int DLL_EXTENSION = 1;

        private const int ANYWHERE_EXTENSION = 2;

        private static readonly string[] extensions =
        {
            ".exe",
            ".dll",
            ".anywhere"
        };

        private readonly IAnyWhereConfigurationFiles files;

        public AnyWhereConfigurationAdapterFactory(
            IAnyWhereConfigurationFiles files)
        {
            this.files = files ?? throw new ArgumentNullException(nameof(files));
        }

        public IEnumerable<AnyWhereConfigurationAdapterProxy> CreateProxies(
            IAnyWhereConfigurationAdapterArguments adapterArguments)
        {
            foreach (var arg in adapterArguments.Enumerate())
            {
                var result = this.files.Find(arg.Definition.AssemblyName, extensions);

                this.ThrowIfResultsAreAmbiguity(arg, result);
                this.ThrowIfResultsAreEmpty(arg, result);

                var adapterAssemblyDirectory = result[result.Count - 1];
                var adapterAssemblyPath = result[EXE_EXTENSION] ?? result[DLL_EXTENSION];

                var adapterType = this.LoadAdapterType(
                    arg,
                    this.LoadAdapterAssembly(arg, adapterAssemblyDirectory, adapterAssemblyPath));

                if (!typeof(IAnyWhereConfigurationAdapter).IsAssignableFrom(adapterType))
                {
                    throw new InvalidOperationException(
                        $"{adapterType.FullName} : type doesn't implement '{nameof(IAnyWhereConfigurationAdapter)}'");
                }

                IAnyWhereConfigurationAdapter adapterInstance;
                try
                {
                    adapterInstance = (IAnyWhereConfigurationAdapter) Activator.CreateInstance(adapterType);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(
                        $"Could not create adapter's instance of '{adapterType.FullName}' type.",
                        ExceptionDispatchInfo.Capture(e).SourceException);
                }

                yield return new AnyWhereConfigurationAdapterProxy(adapterInstance, arg.Environment, arg.Definition);
            }
        }

        private Assembly LoadAdapterAssembly(
            AnyWhereConfigurationAdapterArgument arg,
            string adapterAssemblyDirectory,
            string adapterAssemblyPath)
        {
            var adapterAssemblyLoadContext = new AdapterAssemblyLoadContext(adapterAssemblyDirectory, this.files);
            try
            {
                return adapterAssemblyLoadContext.LoadFromAssemblyPath(adapterAssemblyPath);
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(
                    $"Could not load adapter's assembly '{arg.Definition.AssemblyName}'"
                  + $" or it's dependencies from '{adapterAssemblyDirectory}' directory.",
                    e.FileName,
                    ExceptionDispatchInfo.Capture(e).SourceException);
            }
        }

        private Type LoadAdapterType(
            AnyWhereConfigurationAdapterArgument arg,
            Assembly adapterAssembly)
        {
            try
            {
                return adapterAssembly.GetType(arg.Definition.TypeName, true);
            }
            catch (TypeLoadException e)
            {
                throw new TypeLoadException(
                    $"Could not load adapter's type '{arg.Definition.TypeName}'"
                  + $" from '{adapterAssembly.Location}' assembly.",
                    ExceptionDispatchInfo.Capture(e).SourceException);
            }
        }

        private void ThrowIfResultsAreAmbiguity(
            AnyWhereConfigurationAdapterArgument arg,
            IReadOnlyList<string> result)
        {
            if (result.Count > extensions.Length + 1)
            {
                var sb = new StringBuilder()
                   .AppendFormat("Found multiple files corresponding to '{0}' adapter in different directories:", arg.Definition.AssemblyName)
                   .AppendLine();

                for (var i = 0; i < result.Count; i += extensions.Length + 1)
                {
                    for (var j = i; j < extensions.Length; ++j)
                    {
                        if (result[j] != null)
                        {
                            sb.Append("- ").AppendLine(result[EXE_EXTENSION]);
                        }
                    }
                }

                throw new InvalidOperationException(sb.ToString());
            }
        }

        private void ThrowIfResultsAreEmpty(
            AnyWhereConfigurationAdapterArgument arg,
            IReadOnlyList<string> result)
        {
            if (result.Count == 0 || result[EXE_EXTENSION] is null && result[DLL_EXTENSION] is null)
            {
                var sb = new StringBuilder()
                   .AppendFormat("Cannot find '{0}' adapter's assembly in any of probing paths:", arg.Definition.AssemblyName)
                   .AppendLine();

                foreach (var directory in this.files.Directories)
                {
                    sb.Append("- ").AppendLine(directory);
                }

                throw new FileNotFoundException(sb.ToString(), arg.Definition.AssemblyName);
            }
        }
    }
}